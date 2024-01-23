using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using RabbitMQ.Client;
using System.Text.Json;
using VacancieAPI.RabbitMq;
using VacancieAPI.ServiceGrpc;
using VacancieAPI.VacancieRpc;
using VacancieAPI.ViewModel;
using VacancieDomain.Model;
using VacancieRepository;
using VacancieService.VacancieService;
using VacancieService.VacancyService;

namespace VacancieAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VacancieController : ControllerBase
    {
        private readonly IVacancieService _VacancieService;
        private readonly LocationRpc _rpc;
        private readonly CompanyRpc _companyRpc;
        IDistributedCache cache;
        public VacancieController(IVacancieService VacancieService, LocationRpc rpc, CompanyRpc companyRpc, IModel? _rabbitMqChannel, IDistributedCache distributed)
        {
            _VacancieService = VacancieService;
            _rpc = rpc;
            _companyRpc = companyRpc;
            cache = distributed;
        }
        [HttpPost("CreateVacancie")]
        [Authorize(Roles = "Admin,Moder,Company")]
        public async Task<IActionResult> CreateVacancie(VacancieViewModel model)
        {
            VacancieModel language = new VacancieModel
            {
               CityId =  GetCityId(model.CityId),
                CountryId =  GetCountryId(model.CountryId),
                AboutWork = model.AboutWork.Trim(),
                WorkName = model.WorkName.Trim(),
                CompanyId =  GetCompanyId(model.CompanyId),
                Salary = model.Salary,
                Pinned = 0,
            };
            if (model.CompanyId != 0 && model.WorkName != null)
            {
                await _VacancieService.CreateVacancie(language);
                return CreatedAtAction("SingleVacancie", new { id = language.Id }, model);
            }
            return BadRequest("Не все обязательные поля были заполнены");
        }
        [HttpPut("EditVacancie/{id}")]
        [Authorize(Roles = "Admin,Moder,Company")]
        public async Task<ActionResult<VacancieViewModel>> EditVacancie(int id, VacancieViewModel model)
        {
            VacancieModel Vacancie = await _VacancieService.GetVacancie(id);
            if (ModelState.IsValid)
            {
                Vacancie.CityId =  GetCityId(model.CityId);
                Vacancie.CountryId =  GetCountryId(model.CountryId);
                Vacancie.AboutWork = model.AboutWork.Trim();
                Vacancie.WorkName = model.WorkName.Trim();
               Vacancie.CompanyId =  GetCompanyId(model.CompanyId);
                Vacancie.Salary = model.Salary;
                if (model.CompanyId != 0 && model.WorkName != null)
                {
                    await _VacancieService.UpdateVacancie(Vacancie);
                    return Ok(model);
                }
            }
            return BadRequest(ModelState);
        }
        [HttpDelete("DeleteVacancie/{id}")]
        [Authorize(Roles = "Admin,Moder,Company")]
        public async Task<ActionResult<VacancieViewModel>> DeleteVacancie(int id)
        {
            await _VacancieService.DeleteVacancie(id);
            return Ok("Вакансия успешно удалена");
        }
        [HttpGet("GetOneVacancie/{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<VacancieViewModel>> SingleVacancie(int id)
        {
            VacancieViewModel model = new VacancieViewModel();
            if (id != 0)
            {
                var userId = await cache.GetStringAsync(id.ToString());
                if (userId != null) model = JsonSerializer.Deserialize<VacancieViewModel>(userId)!;
                if (model.Id == 0)
                {
                    VacancieModel Vacancie = await _VacancieService.GetVacancie(id);
                    if (Vacancie != null)
                    {

                        model.CityId = Vacancie.CityId;
                        model.CityName = GetCityName(Vacancie.CityId);
                        model.CompanyId = Vacancie.CompanyId;
                        model.CompanyName = GetCompanyName(Vacancie.CompanyId);
                        model.CountryId = Vacancie.CountryId;
                        model.CountryName = GetCountryName(Vacancie.CountryId);
                        model.AboutWork = Vacancie.AboutWork.Trim();
                        model.WorkName = Vacancie.WorkName.Trim();
                        model.Salary = Vacancie.Salary;
                        model.Id = Vacancie.Id;
                        userId = JsonSerializer.Serialize(model);
                        await cache.SetStringAsync(model.Id.ToString(), userId, new DistributedCacheEntryOptions
                        {
                            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
                        });
                        return new ObjectResult(model);
                    }
                }
                else { return new ObjectResult(model); }
                return BadRequest("Вакансия не найдена");
            }
            return BadRequest();
        }
        [HttpGet("GetAllVacancie")]
        [AllowAnonymous]
        public IEnumerable<VacancieViewModel> Index()
        {
            List<VacancieViewModel> model = new List<VacancieViewModel>();
            if (_VacancieService != null)
            {
                _VacancieService.GetAll().ToList().ForEach(u =>
                {
                    VacancieViewModel Vacancie = new VacancieViewModel
                    {
                        Id = u.Id,
                        CityId = u.CityId,
                        CompanyId = u.CompanyId,
                        CountryId = u.CountryId,
                        AboutWork = u.AboutWork,
                        WorkName = u.WorkName,
                        Salary = u.Salary,
                         CountryName =  GetCountryName(u.CountryId),
                         CityName =  GetCityName(u.CityId),
                          CompanyName =  GetCompanyName(u.CompanyId),
                          Pinned = u.Pinned,
                    };
                    model.Add(Vacancie);
                });
            }
            return model;
        }
        private int GetCityId(int id)
        {
            var city = _rpc.GetCityById(id);
            return (int)city.Id;
        }
        private int GetCountryId(int id)
        {
            var country =  _rpc.GetCountryById(id);
            return (int)country.Id;
        }
        private int GetCompanyId(int id)
        {
            var company = _companyRpc.GetCompany(id);
            return (int)company.Id;
        }
        private string GetCountryName(int id)
        {
            var country = _rpc.GetCountryById(id);
            return country.CountryName;
        }
        private string GetCityName(int id)
        {
            var city = _rpc.GetCityById(id);
            return city.CityName;
        }
        private string GetCompanyName(int id)
        {
            var company = _companyRpc.GetCompany(id);
            return company.CompanyName;
        }
    }
}
