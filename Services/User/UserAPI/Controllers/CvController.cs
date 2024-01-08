using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using UserAPI.RabbitMq;
using UserAPI.ServiceGrpc;
using UserAPI.ViewModel;
using UserDomain.Models;
using UserService.CvService;
using UserService.LanguageService;
using UserService.UserService;

namespace UserAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CvController : ControllerBase
    {
        private readonly ICvService _cvService;
        private readonly ILanguageService _languageService;
        private readonly IUserService _userService;
        private readonly CategoryRpc _rpc;
        private readonly IResponseProducer _responseProducer;
        public CvController(ICvService cvService, ILanguageService languageService, IUserService userService, CategoryRpc rpc, IResponseProducer producer, IModel? _rabbitMqChannel)
        {
            _cvService = cvService;
            _languageService = languageService;
            _userService = userService;
            _rpc = rpc;
            _responseProducer = producer;
        }
        [HttpPost("CreateCv")]
        public async Task<IActionResult> CreateCv(CvViewModel model)
        {
            CvModel cv = new CvModel
            {
                LanguageId = await GetLanguageId(model.LanguageId),
                UserId = model.UserId, 
                AboutMe = model.AboutMe.Trim(),
                JobNmae = model.JobNmae.Trim(),
                CategoryId = GetCategoryId(model.CategoryId),
                Salary = model.Salary,
            };
            if (model.JobNmae != null)
            {
                await _cvService.CreateCv(cv);
                return CreatedAtAction("UserCv", new { id = cv.Id }, model);
            }
            return BadRequest("Не все обязательные поля были заполнены");
        }
        [HttpPut("EditCv/{id}")]
        public async Task<ActionResult<CvViewModel>> EditCv(int id, CvViewModel model)
        {
            CvModel cv = await _cvService.GetCv(id);
            if (ModelState.IsValid)
            {
                cv.AboutMe = model.AboutMe.Trim();
                cv.JobNmae = model.JobNmae.Trim();
                cv.Salary = model.Salary;
                cv.LanguageId = await GetLanguageId(model.LanguageId);
                if (model.JobNmae != null)
                {
                    await _cvService.UpdateCv(cv);
                    return Ok(model);
                }
            }
            return BadRequest(ModelState);
        }
        [HttpDelete("DeleteCv/{id}")]
        public async Task<ActionResult<CvViewModel>> DeleteCv(int id)
        {
            await _cvService.DeleteCv(id);
            return Ok("Резюме успешно удалено");
        }
        [HttpGet("GetOneCv/{id}")]
        public async Task<ActionResult<CvViewModel>> UserCv(int id)
        {
            CvViewModel model = new CvViewModel();
            if (id != 0)
            {
                CvModel CvEntity = await _cvService.GetCv(id);
                if (CvEntity != null)
                {
                    model.AboutMe = CvEntity.AboutMe;
                    model.JobNmae = CvEntity.JobNmae;
                    model.LanguageId = CvEntity.LanguageId;
                    model.LanguageName = await GetLanguageName(model.LanguageId);
                    model.UserId = CvEntity.UserId;
                    model.UserName = await GetUserName(model.UserId);
                    model.Id = CvEntity.Id;
                    model.CategoryId = CvEntity.CategoryId;
                    model.CategoryName = await GetCategoryName(model.CategoryId);
                    model.Salary = CvEntity.Salary;
                    return new ObjectResult(model);
                }
                return BadRequest("Резюме не найдено");
            }
            return BadRequest();
        }
        [HttpGet("GetAllCv")]
        public IEnumerable<CvViewModel> Index()
        {
            List<CvViewModel> model = new List<CvViewModel>();
            if (_cvService != null)
            {
                _cvService.GetAll().ToList().ForEach(u =>
                {
                    CvViewModel cv = new CvViewModel
                    {
                        Id = u.Id,
                        JobNmae = u.JobNmae,
                        LanguageId = u.LanguageId,
                        AboutMe = u.AboutMe,
                        UserId = u.UserId,
                        CategoryId = u.CategoryId,
                        Salary = u.Salary,
                    };
                    model.Add(cv);
                });
            }
            return model;
        }
        [HttpGet("GetResponse/{Responseid}")]
        public async Task<ActionResult<ResponseViewModel>> SingleResponse(int Responseid)
        {

            ResponseViewModel model = new ResponseViewModel();
            model = await _responseProducer.TakeSingleResponseForUser(Responseid);
            if (model != null)
            {
                return model;
            }
            return BadRequest();
        }
        [HttpPost("CreateResponse")]
        public async Task<IActionResult> CreateResponse(ResponseViewModel model)
        {
            ResponseViewModel Response = new ResponseViewModel
            {
                CvId = model.CvId,
                VacancieId = model.VacancieId,
                Message = model.Message.Trim(),
            };
            if (model.Message != null)
            {
                string answer = await _responseProducer.CreateReposneForUser(Response);
                return Ok(answer);
            }
            return BadRequest("Не все обязательные поля были заполнены");
        }
        [HttpPut("EditResponse")]
        public async Task<IActionResult> EditResponse(ResponseViewModel model)
        {
            ResponseViewModel Response = new ResponseViewModel
            {
                CvId = model.CvId,
                VacancieId = model.VacancieId,
                Message = model.Message,
                Id = model.Id
            };
            if (model.Message != null)
            {
                string answer = await _responseProducer.EditResponseForUser(Response);
                return Ok(answer);
            }
            return BadRequest("Не все обязательные поля были заполнены");
        }
        [HttpGet("GetResponseForUser/{CvId}")]
        public IEnumerable<ResponseViewModel> AllForresponseResponse(int CvId)
        {
            var model = _responseProducer.TakeAllResponseOfUser(CvId);
            return model;
        }
        [HttpDelete("DeleteResponse/{id}")]
        public async Task<ActionResult<ResponseViewModel>> DeleteResponse(int id)
        {
            await _responseProducer.DeleteResponseForUser(id);
            return Ok("Вакансия успешно удалена");
        }
        private int GetCategoryId(int id)
        {
            int category = _rpc.GetCategoryById(id);
            return category;
        }
        private async Task<string> GetCategoryName(int id)
        {
            var model = await _rpc.GetCategoryModel(id);
            return model.CategoryName;
        }
        private async Task<string> GetLanguageName(int id)
        {
            LanguageModel language = await _languageService.GetLanguage(id);
            return language.Language;
        }
        private async Task<int> GetLanguageId(int id)
        {
            LanguageModel language = await _languageService.GetLanguage(id);
            return language.Id;
        }
        private async Task<string> GetUserName(int id)
        {
            UserModel user = await _userService.GetUser(id);
            return user.Name;
        }
    }
}
