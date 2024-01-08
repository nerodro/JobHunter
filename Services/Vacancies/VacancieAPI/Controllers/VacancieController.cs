using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
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
        public VacancieController(IVacancieService VacancieService, LocationRpc rpc, CompanyRpc companyRpc, IModel? _rabbitMqChannel)
        {
            _VacancieService = VacancieService;
            _rpc = rpc;
            _companyRpc = companyRpc;
        }
        [HttpPost("CreateVacancie")]
        [Authorize(Roles = "Admin,Moder,Company")]
        public async Task<IActionResult> CreateVacancie(VacancieViewModel model)
        {
            VacancieModel language = new VacancieModel
            {
                CityId = await GetCityId(model.CityId),
                CountryId = await GetCountryId(model.CountryId),
                AboutWork = model.AboutWork.Trim(),
                WorkName = model.WorkName.Trim(),
                CompanyId = await GetCompanyId(model.CompanyId),
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
                Vacancie.CityId = await GetCityId(model.CityId);
                Vacancie.CountryId = await GetCountryId(model.CountryId);
                Vacancie.AboutWork = model.AboutWork.Trim();
                Vacancie.WorkName = model.WorkName.Trim();
                Vacancie.CompanyId = await GetCompanyId(model.CompanyId);
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
                VacancieModel Vacancie = await _VacancieService.GetVacancie(id);
                if (Vacancie != null)
                {

                    model.CityId =  Vacancie.CityId;
                    model.CityName = await GetCityName(Vacancie.CityId);
                    model.CompanyId = Vacancie.CompanyId;
                    model.CompanyName = await GetCompanyName(Vacancie.CompanyId);
                    model.CountryId = Vacancie.CountryId;
                    model.CountryName = await GetCountryName(Vacancie.CountryId);
                    model.AboutWork = Vacancie.AboutWork.Trim();
                    model.WorkName = Vacancie.WorkName.Trim();
                    model.Id = Vacancie.Id;
                    return new ObjectResult(model);
                }
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
                    };
                    model.Add(Vacancie);
                });
            }
            return model;
        }
        private async Task<int> GetCityId(int id)
        {
            var city = await _rpc.GetCityById(id);
            return (int)city.Id;
        }
        private async Task<int> GetCountryId(int id)
        {
            var country = await _rpc.GetCountryById(id);
            return (int)country.Id;
        }
        private async Task<int> GetCompanyId(int id)
        {
            var company = await _companyRpc.GetCompany(id);
            return (int)company.Id;
        }
        private async Task<string> GetCountryName(int id)
        {
            var country = await _rpc.GetCountryById(id);
            return country.CountryName;
        }
        private async Task<string> GetCityName(int id)
        {
            var city = await _rpc.GetCityById(id);
            return city.CityName;
        }
        private async Task<string> GetCompanyName(int id)
        {
            var company = await _companyRpc.GetCompany(id);
            return company.CompanyName;
        }
        private IConnection GetRabbitMqConnection()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            return factory.CreateConnection();
        }
    }
}
