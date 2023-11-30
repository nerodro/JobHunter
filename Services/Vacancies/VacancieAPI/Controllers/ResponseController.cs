using Microsoft.AspNetCore.Mvc;
using VacancieAPI.ViewModel;
using VacancieDomain.Model;
using VacancieService.ResponseService;

namespace VacancieAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ResponseController : ControllerBase
    {
        private readonly IResponseService _ResponseService;
        public ResponseController(IResponseService ResponseService)
        {
            _ResponseService = ResponseService;
        }
        [HttpPost("CreateResponse")]
        public async Task<IActionResult> CreateResponse(ResponseViewModel model)
        {
            ResponseModel language = new ResponseModel
            {
                CvId = model.CvId,
                VacancieId = model.VacancieId,
                Message = model.Message.Trim(),
            };
            if (model.CvId != 0 && model.VacancieId != 0)
            {
                await _ResponseService.CreateResponse(language);
                return CreatedAtAction("SingleResponse", new { id = language.Id }, model);
            }
            return BadRequest("Не все обязательные поля были заполнены");
        }
        [HttpPut("EditResponse/{id}")]
        public async Task<ActionResult<ResponseViewModel>> EditResponse(int id, ResponseViewModel model)
        {
            ResponseModel Response = await _ResponseService.GetResponse(id);
            if (ModelState.IsValid)
            {
                Response.Message = model.Message.Trim();
                Response.CvId = model.CvId;
                Response.VacancieId = model.VacancieId;
                if (model.CvId != 0 && model.VacancieId != 0)
                {
                    await _ResponseService.UpdateResponse(Response);
                    return Ok(model);
                }
            }
            return BadRequest(ModelState);
        }
        [HttpDelete("DeleteResponse/{id}")]
        public async Task<ActionResult<ResponseViewModel>> DeleteResponse(int id)
        {
            await _ResponseService.DeleteResponse(id);
            return Ok("Отклик успешно удален");
        }
        [HttpGet("GetOneResponse/{id}")]
        public async Task<ActionResult<ResponseViewModel>> SingleResponse(int id)
        {
            ResponseViewModel model = new ResponseViewModel();
            if (id != 0)
            {
                ResponseModel Response = await _ResponseService.GetResponse(id);
                if (Response != null)
                {
                    model.Message = Response.Message;
                    model.CvId = Response.CvId;
                    model.VacancieId = Response.VacancieId;
                    model.Id = Response.Id;
                    return new ObjectResult(model);
                }
                return BadRequest("Отклик не найден");
            }
            return BadRequest();
        }
        [HttpGet("GetAllResponse")]
        public IEnumerable<ResponseViewModel> Index()
        {
            List<ResponseViewModel> model = new List<ResponseViewModel>();
            if (_ResponseService != null)
            {
                _ResponseService.GetAll().ToList().ForEach(u =>
                {
                    ResponseViewModel Response = new ResponseViewModel
                    {
                        Id = u.Id,
                        Message = u.Message,
                        CvId = u.CvId,
                        VacancieId=u.VacancieId,
                    };
                    model.Add(Response);
                });
            }
            return model;
        }
    }
}
