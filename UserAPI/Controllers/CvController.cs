using Microsoft.AspNetCore.Mvc;
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
        public CvController(ICvService cvService, ILanguageService languageService, IUserService userService, CategoryRpc rpc)
        {
            _cvService = cvService;
            _languageService = languageService;
            _userService = userService;
            _rpc = rpc;
        }
        [HttpPost("CreateCv")]
        public async Task<IActionResult> CreateCv(CvViewModel model)
        {
            Task<int> catId = GetCategoryId(model.CategoryId);
            if(catId.Result == 0)
            {
                return BadRequest("Указанная категория не найдена");
            }
            CvModel cv = new CvModel
            {
                LanguageId = model.LanguageId,
                UserId = model.UserId, 
                AboutMe = model.AboutMe.Trim(),
                JobNmae = model.JobNmae.Trim(),
                CategoryId = model.CategoryId,
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
                cv.LanguageId = model.LanguageId;
                if (model.JobNmae != null)
                {
                    await _cvService.UpdateCv(cv);
                    return Ok(model);
                }
            }
            return BadRequest(ModelState);
        }
        [HttpDelete("DeleteCv/{id}")]
        public async Task<ActionResult<CvViewModel>> DeleteUser(int id)
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
                    };
                    model.Add(cv);
                });
            }
            return model;
        }

        //Только тестовый метод, удалить перед окончанием работы над бэком
        [HttpGet("GetCategorey/{id}")]
        public IActionResult GetCategory(int id)
        {
            var cat = _rpc.GetCategoryById(id);
            if(cat != 0)
            {
                return Ok();
            }
            else
            {
                return BadRequest("Указанная категория не найдена");
            }
        }
        private async Task<int> GetCategoryId(int id)
        {
            int category = _rpc.GetCategoryById(id);
            if(category != 0)
            {
                return await Task.FromResult(category);
            }
            else
            {
                return await Task.FromResult(0);
            }
        }
        private async Task<string> GetLanguageName(int id)
        {
            LanguageModel language = await _languageService.GetLanguage(id);
            string name = language.Language;
            return name;
        }
        private async Task<string> GetUserName(int id)
        {
            UserModel user = await _userService.GetUser(id);
            string name = user.Name;
            return name;
        }
    }
}
