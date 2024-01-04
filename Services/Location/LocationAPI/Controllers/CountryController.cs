using LocationAPI.ViewModel;
using LocationDomain.Model;
using LocationService.CountryService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LocationAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CountryController : ControllerBase
    {
        private readonly ICountryService _Country;
        public CountryController(ICountryService Country)
        {
            _Country = Country;
        }
        [HttpPost("CreateCountry")]
        [Authorize(Roles = "Admin,Moder")]
        public async Task<IActionResult> CreateCountry(CountryViewModel model)
        {
            CountryModel Country = new CountryModel
            {
                CountryName = model.CountryName.Trim(),
            };
            if (model.CountryName != null)
            {
                await _Country.CreateCountry(Country);
                return CreatedAtAction("SingleCountry", new { id = Country.Id }, model);
            }
            return BadRequest("Не все обязательные поля были заполнены");
        }
        [HttpPut("EditCountry/{id}")]
        [Authorize(Roles = "Admin,Moder")]
        public async Task<ActionResult<CountryViewModel>> EditCountry(int id, CountryViewModel model)
        {
            CountryModel Country = await _Country.GetCountry(id);
            if (ModelState.IsValid)
            {
                Country.CountryName = model.CountryName.Trim();
                if (model.CountryName != null)
                {
                    await _Country.UpdateCountry(Country);
                    return Ok(model);
                }
            }
            return BadRequest(ModelState);
        }
        [HttpDelete("DeleteCountry/{id}")]
        [Authorize(Roles = "Admin,Moder")]
        public async Task<ActionResult<CountryViewModel>> DeleteCountry(int id)
        {
            await _Country.DeleteCountry(id);
            return Ok("Город успешно удален");
        }
        [HttpGet("GetOneCountry/{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<CountryViewModel>> SingleCountry(int id)
        {
            CountryViewModel model = new CountryViewModel();
            if (id != 0)
            {
                CountryModel Country = await _Country.GetCountry(id);
                if (Country != null)
                {
                    model.CountryName = Country.CountryName;
                    model.Id = Country.Id;
                    return new ObjectResult(model);
                }
                return BadRequest("Город не найден");
            }
            return BadRequest();
        }
        [HttpGet("GetAllCountry")]
        [AllowAnonymous]
        public IEnumerable<CountryViewModel> Index()
        {
            List<CountryViewModel> model = new List<CountryViewModel>();
            if (_Country != null)
            {
                _Country.GetAllCountry().ToList().ForEach(u =>
                {
                    CountryViewModel Country = new CountryViewModel
                    {
                        Id = u.Id,
                        CountryName = u.CountryName,
                    };
                    model.Add(Country);
                });
            }
            return model;
        }
    }
}
