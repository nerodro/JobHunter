using Microsoft.AspNetCore.Mvc;
using UserAPI.ViewModel;
using UserDomain.Models;
using UserService.CvService;

namespace UserAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CvController : ControllerBase
    {
        private readonly ICvService _cvService;
        public CvController(ICvService cvService)
        {
            _cvService = cvService;
        }
        [HttpPost("CreateCv")]
        public async Task<IActionResult> CreateCv(CvViewModel model)
        {
            CvModel cv = new CvModel
            {
                LanguageId = model.LanguageId,
                UserId = model.UserId, 
                AboutMe = model.AboutMe,
                JobNmae = model.JobNmae,
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
                cv.AboutMe = model.AboutMe;
                cv.JobNmae = model.JobNmae;
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
    }
}
