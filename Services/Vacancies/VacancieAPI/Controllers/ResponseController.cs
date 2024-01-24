using HotChocolate.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using RabbitMQ.Client;
using System.Text.Json;
using VacancieAPI.VacancieRpc;
using VacancieAPI.ViewModel;
using VacancieDomain.Model;
using VacancieService.ResponseService;
using VacancieService.VacancieService;

namespace VacancieAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ResponseController : ControllerBase
    {
        private readonly IResponseService _ResponseService;
        private readonly CvRpc _rpc;
        private readonly IVacancieService _VacancieService;
        IDistributedCache _cache;
        public ResponseController(IResponseService ResponseService, CvRpc cv, IVacancieService vacancie, IModel? _rabbitMqChannel, IDistributedCache cache)
        {
            _ResponseService = ResponseService;
            _rpc = cv;
            _VacancieService = vacancie;
            _cache = cache;
        }
        [HttpPost("CreateResponse")]
        [Authorize(Roles = "Admin,Moder,User")]
        public async Task<IActionResult> CreateResponse(ResponseViewModel model)
        {
            ResponseModel language = new ResponseModel
            {
                CvId = GetCvId(model.CvId),
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
        [Authorize(Roles = "Admin,Moder,User")]
        public async Task<ActionResult<ResponseViewModel>> EditResponse(int id, ResponseViewModel model)
        {
            ResponseModel Response = await _ResponseService.GetResponse(id);
            if (ModelState.IsValid)
            {
                Response.Message = model.Message.Trim();
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
        [Authorize(Roles = "Admin,Moder,User")]
        public async Task<ActionResult<ResponseViewModel>> DeleteResponse(int id)
        {
            var model = await _ResponseService.GetResponse(id);
            if ( model != null)
            {
                await _ResponseService.DeleteResponse(id);
                return Ok("Отклик успешно удален");
            }
            return BadRequest(ModelState);
            
        }
        [HttpGet("GetOneResponse/{id}")]
        [Authorize(Roles = "Admin,Moder,User")]
        public async Task<ActionResult<ResponseViewModel>> SingleResponse(int id)
        {
            ResponseViewModel model = new ResponseViewModel();
            if (id != 0)
            {
                var responseId = await _cache.GetStringAsync(id.ToString());
                if (responseId != null) model = JsonSerializer.Deserialize<ResponseViewModel>(responseId)!;
                if (model.Id == 0)
                {
                    ResponseModel Response = await _ResponseService.GetResponse(id);
                    if (Response != null)
                    {
                        model.Message = Response.Message;
                        model.CvId = Response.CvId;
                        model.CvName = GetCvName(Response.CvId);
                        model.VacancieId = Response.VacancieId;
                        model.VacancieName = await GetVacancieName(Response.VacancieId);
                        model.Id = Response.Id;
                        responseId = JsonSerializer.Serialize(model);
                        await _cache.SetStringAsync(model.Id.ToString(), responseId, new DistributedCacheEntryOptions
                        {
                            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
                        });
                        return new ObjectResult(model);
                    }
                    return BadRequest("Отклик не найден");
                }
                else
                {
                    return new ObjectResult(model);
                }
            }
            return BadRequest();
        }
        [HttpGet("GetAllResponse")]
        [Authorize(Roles = "Admin,Moder")]
        public IEnumerable<ResponseViewModel> Index()
        {
            List<ResponseViewModel> model = new List<ResponseViewModel>();
            if (_ResponseService != null)
            {
                _ResponseService.GetAll().ToList().ForEach(async u =>
                {
                    ResponseViewModel Response = new ResponseViewModel
                    {
                        Id = u.Id,
                        Message = u.Message,
                        CvId = u.CvId,
                        VacancieId = u.VacancieId,
                        CvName =  GetCvName(u.CvId),
                        VacancieName = await GetVacancieName(u.VacancieId),
                    };
                    model.Add(Response);
                });
            }
            return model;
        }
        private int GetCvId(int id)
        {
            var cv =  _rpc.GetCv(id);
            return (int)cv.Id;
        }
        private string GetCvName(int id)
        {
            var cv =  _rpc.GetCv(id);
            return cv.CvName;
        }
        private async Task<string> GetVacancieName(int id)
        {
            var vacancie = await _VacancieService.GetVacancie(id);
            return vacancie.WorkName;
        }
    }
}
