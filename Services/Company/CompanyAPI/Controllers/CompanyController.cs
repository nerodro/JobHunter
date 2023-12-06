using CompanyAPI.RabbitMq;
using CompanyAPI.ServiceGrpc;
using CompanyAPI.ViewModel;
using CompanyDomain.Model;
using CompanyService.CompanyService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Numerics;
using System.Text;

namespace CompanyAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _CompanyService;
        private readonly CategoryRpc _rpc;
        private readonly LocationRpc _Locrpc;
        private readonly ICompanyProducer _companyProducer;
        private IModel _rabbitMqChannel;
        public CompanyController(ICompanyService CompanyService, CategoryRpc categoryGrpc, LocationRpc Locrpc, ICompanyProducer company, IModel rabbitMqChannel)
        {
            _CompanyService = CompanyService;
            _rpc = categoryGrpc;
            _Locrpc = Locrpc;
            _companyProducer = company;
            _rabbitMqChannel = rabbitMqChannel;
            var factory = new ConnectionFactory() { HostName = "localhost" };
            var connection = factory.CreateConnection();
            _rabbitMqChannel = connection.CreateModel();
        }
        [HttpPost("CreateCompany")]
        public async Task<IActionResult> CreateCompany(CompanyViewModel model)
        {
            CompanyModel language = new CompanyModel
            {
                CompanyName = model.CompanyName.Trim(),
                Email = model.Email.Trim(),
                CityId = await GetCityId(model.CityId),
                CountryId = await GetCountryId(model.CountryId),
                Password = model.Password.Trim(),
                Phone = model.Phone,
                CategoryId = await GetCategoryId(model.CategoryId),
            };
            if (model.CompanyName != null)
            {
                await _CompanyService.CreateCompany(language);
                return CreatedAtAction("SingleCompany", new { id = language.Id }, model);
            }
            return BadRequest("Не все обязательные поля были заполнены");
        }
        [HttpPut("EditCompany/{id}")]
        public async Task<ActionResult<CompanyViewModel>> EditCompany(int id, CompanyViewModel model)
        {
            CompanyModel Company = await _CompanyService.GetCompany(id);
            if (ModelState.IsValid)
            {
                Company.CompanyName = model.CompanyName.Trim();
                Company.Email = model.Email.Trim();
                Company.CityId = await GetCityId(model.CityId);
                Company.CountryId = await GetCountryId(model.CountryId);
                Company.Password = model.Password.Trim();
                Company.Phone = model.Phone;
                Company.CategoryId = await GetCategoryId(model.CategoryId);
                if (model.CompanyName != null)
                {
                    await _CompanyService.UpdateCompany(Company);
                    return Ok(model);
                }
            }
            return BadRequest(ModelState);
        }
        [HttpDelete("DeleteCompany/{id}")]
        public async Task<ActionResult<CompanyViewModel>> DeleteCompany(int id)
        {
            await _CompanyService.DeleteCompany(id);
            return Ok("Компания успешно удалена");
        }
        [HttpGet("GetOneCompany/{id}")]
        public async Task<ActionResult<CompanyViewModel>> SingleCompany(int id)
        {
            CompanyViewModel model = new CompanyViewModel();
            if (id != 0)
            {
                CompanyModel Company = await _CompanyService.GetCompany(id);
                if (Company != null)
                {
                    model.CompanyName = Company.CompanyName;
                    model.Id = Company.Id;
                    model.CountryId = Company.CountryId;
                    model.CountryName = await GetCountryName(Company.CountryId);
                    model.CityId = Company.CityId;
                    model.CityName = await GetCityName(Company.CityId);
                    model.Phone = Company.Phone;
                    model.Email = Company.Email;
                    model.CategoryId = Company.CategoryId;
                    model.CategoryName = await GetCategoryName(Company.CategoryId);
                    return new ObjectResult(model);
                }
                return BadRequest("Компания не найдена");
            }
            return BadRequest();
        }
        
        [HttpGet("GetAllCompany")]
        public IEnumerable<CompanyViewModel> Index()
        {
            List<CompanyViewModel> model = new List<CompanyViewModel>();
            if (_CompanyService != null)
            {
                _CompanyService.GetAllCompany().ToList().ForEach(u =>
                {
                    CompanyViewModel Company = new CompanyViewModel
                    {
                        Id = u.Id,
                        CompanyName = u.CompanyName,
                        CountryId = u.CountryId,
                        CityId = u.CityId,
                        Phone = u.Phone,
                        Email = u.Email,
                        CategoryId = u.CategoryId,
                    };
                    model.Add(Company);
                });
            }
            return model;
        }
        [HttpGet("GetVacancy/{id}")]
        public async Task<ActionResult<VacancieViewModel>> SingleVacancy(int id)
        {

            VacancieViewModel model = new VacancieViewModel();
            model = await _companyProducer.TakeSingleVacancieForCompany(id);
            if (model != null)
            {
                return model;
            }
            return BadRequest();
        }
        [HttpPost("CreateVacancy")]
        public async Task<IActionResult> CreateVacancy(VacancieViewModel model)
        {
            VacancieViewModel vacancy = new VacancieViewModel
            {
                CityId = await GetCityId(model.CityId),
                CountryId = await GetCountryId(model.CountryId),
                AboutWork = model.AboutWork.Trim(),
                WorkName = model.WorkName.Trim(),
                CompanyId = model.CompanyId,
            };
            if (model.CompanyName != null)
            {
                string answer = await _companyProducer.CreateVacancieForCompany(vacancy);
                return Ok(answer);
            }
            return BadRequest("Не все обязательные поля были заполнены");
        }
        private async Task<int> GetCategoryId(int id)
        {
            var category = await _rpc.GetCategory(id);
            return (int)category.Id;
        }
        private async Task<string> GetCategoryName(int id)
        {
            var model = await _rpc.GetCategory(id);
            return model.CategoryName;
        }
        private async Task<int> GetCityId(int id)
        {
            var city = await _Locrpc.GetCityById(id);
            return (int)city.Id;
        }
        private async Task<int> GetCountryId(int id)
        {
            var country = await _Locrpc.GetCountryById(id);
            return (int)country.Id;
        }
        private async Task<string> GetCountryName(int id)
        {
            var country = await _Locrpc.GetCountryById(id);
            return country.CountryName;
        }
        private async Task<string> GetCityName(int id)
        {
            var city = await _Locrpc.GetCityById(id);
            return city.CityName;
        }
    }
}
