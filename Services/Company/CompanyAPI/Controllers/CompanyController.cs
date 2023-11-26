using CompanyAPI.ServiceGrpc;
using CompanyAPI.ViewModel;
using CompanyDomain.Model;
using CompanyService.CompanyService;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;

namespace CompanyAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _CompanyService;
        private readonly CategoryRpc _rpc;
        public CompanyController(ICompanyService CompanyService, CategoryRpc categoryGrpc)
        {
            _CompanyService = CompanyService;
            _rpc = categoryGrpc;
        }
        [HttpPost("CreateCompany")]
        public async Task<IActionResult> CreateCompany(CompanyViewModel model)
        {
            CompanyModel language = new CompanyModel
            {
                CompanyName = model.CompanyName.Trim(),
                Email = model.Email.Trim(),
                CityId = model.CityId,
                CountryId = model.CountryId,
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
                Company.CityId = model.CityId;
                Company.CountryId = model.CountryId;
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
                    model.CityId = Company.CityId;
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
    }
}
