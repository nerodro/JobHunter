using LocationAPI.ViewModel;
using LocationDomain.Model;
using LocationService.CityService;
using Microsoft.AspNetCore.Mvc;

namespace LocationAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CityController : ControllerBase
    {
        private readonly ICityService _City;
        public CityController(ICityService City)
        {
            _City = City;
        }
        [HttpPost("CreateCity")]
        public async Task<IActionResult> CreateCity(CityViewModel model)
        {
            CityModel City = new CityModel
            {
                CityName = model.CityName.Trim(),
                CountryId = model.CountryId
            };
            if (model.CityName != null)
            {
                await _City.CreateCity(City);
                return CreatedAtAction("SingleCity", new { id = City.Id }, model);
            }
            return BadRequest("Не все обязательные поля были заполнены");
        }
        [HttpPut("EditCity/{id}")]
        public async Task<ActionResult<CityViewModel>> EditCity(int id, CityViewModel model)
        {
            CityModel City = await _City.GetCity(id);
            if (ModelState.IsValid)
            {
                City.CityName = model.CityName.Trim();
                City.CountryId = model.CountryId;
                if (model.CityName != null)
                {
                    await _City.UpdateCity(City);
                    return Ok(model);
                }
            }
            return BadRequest(ModelState);
        }
        [HttpDelete("DeleteCity/{id}")]
        public async Task<ActionResult<CityViewModel>> DeleteCity(int id)
        {
            await _City.DeleteCity(id);
            return Ok("Язык успешно удален");
        }
        [HttpGet("GetOneCity/{id}")]
        public async Task<ActionResult<CityViewModel>> SingleCity(int id)
        {
            CityViewModel model = new CityViewModel();
            if (id != 0)
            {
                CityModel City = await _City.GetCity(id);
                if (City != null)
                {
                    model.CityName = City.CityName;
                    model.Id = City.Id;
                    model.CountryId = City.CountryId;
                    return new ObjectResult(model);
                }
                return BadRequest("Язык не найден");
            }
            return BadRequest();
        }
        [HttpGet("GetAllCity")]
        public IEnumerable<CityViewModel> Index()
        {
            List<CityViewModel> model = new List<CityViewModel>();
            if (_City != null)
            {
                _City.GetAllCity().ToList().ForEach(u =>
                {
                    CityViewModel City = new CityViewModel
                    {
                        Id = u.Id,
                        CityName = u.CityName,
                        CountryId = u.CountryId,
                    };
                    model.Add(City);
                });
            }
            return model;
        }
    }
}
