using LocationAPI.ViewModel;
using LocationDomain.Model;
using LocationService.CityService;
using LocationService.CountryService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LocationAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CityController : ControllerBase
    {
        private readonly ICityService _City;
        private readonly ICountryService _Country;
        public CityController(ICityService City, ICountryService country)
        {
            _City = City;
            _Country = country;
        }
        [HttpPost("CreateCity")]
        [Authorize(Roles = "Admin,Moder")]
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
        [Authorize(Roles = "Admin,Moder")]
        public async Task<IActionResult> EditCity(int id, CityViewModel model)
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
        [Authorize(Roles = "Admin,Moder")]
        public async Task<ActionResult<CityViewModel>> DeleteCity(int id)
        {
            var model = await _City.GetCity(id);
            if (model != null)
            {
                await _City.DeleteCity(id);
                return Ok("Страна успешно удалена");
            }
            return BadRequest();
        }
        [HttpGet("GetOneCity/{id}")]
        [AllowAnonymous]
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
                    model.CountryName = await GetCountryName(model.CountryId);
                    return new ObjectResult(model);
                }
                return BadRequest("Страна не найдена");
            }
            return BadRequest();
        }
        [HttpGet("GetAllCity")]
        [AllowAnonymous]
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
        private async Task<string> GetCountryName(int id)
        {
            CountryModel country = await _Country.GetCountry(id);
            string name = country.CountryName;
            return name;
        }
    }
}
