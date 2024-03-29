﻿using CompanyAPI.RabbitMq;
using CompanyAPI.ServiceGrpc;
using CompanyAPI.ViewModel;
using CompanyDomain.Model;
using CompanyService.CompanyService;
using Microsoft.AspNetCore.Authorization;
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
        public CompanyController(ICompanyService CompanyService, CategoryRpc categoryGrpc, LocationRpc Locrpc, ICompanyProducer company)
        {
            _CompanyService = CompanyService;
            _rpc = categoryGrpc;
            _Locrpc = Locrpc;
            _companyProducer = company;
        }
        [HttpPost("CreateCompany")]
        [Authorize(Roles = "Admin,Moder,Company")]
        public async Task<IActionResult> CreateCompany(CompanyViewModel model)
        {
            CompanyModel company = new CompanyModel
            {
                CompanyName = model.CompanyName.Trim(),
                Email = model.Email.Trim(),
                CityId =  GetCityId(model.CityId),
                CountryId = GetCountryId(model.CountryId),
                Password = model.Password.Trim(),
                Phone = model.Phone,
                CategoryId =  GetCategoryId(model.CategoryId),
                RoleId = model.RoleId,
            };
            if (model.CompanyName != null)
            {
                await _CompanyService.CreateCompany(company);
                return CreatedAtAction("SingleCompany", new { id = company.Id }, model);
            }
            return BadRequest("Не все обязательные поля были заполнены");
        }
        [HttpPut("EditCompany/{id}")]
        [Authorize(Roles = "Admin,Moder,Company")]
        public async Task<ActionResult<CompanyViewModel>> EditCompany(int id, CompanyViewModel model)
        {
            CompanyModel Company = await _CompanyService.GetCompany(id);
            if (ModelState.IsValid)
            {
                Company.CompanyName = model.CompanyName.Trim();
                Company.Email = model.Email.Trim();
                Company.CityId =  GetCityId(model.CityId);
                Company.CountryId =  GetCountryId(model.CountryId);
                Company.Password = model.Password.Trim();
                Company.Phone = model.Phone;
                Company.CategoryId =  GetCategoryId(model.CategoryId);
                if (model.CompanyName != null)
                {
                    await _CompanyService.UpdateCompany(Company);
                    return Ok(model);
                }
            }
            return BadRequest(ModelState);
        }
        [HttpDelete("DeleteCompany/{id}")]
        [Authorize(Roles = "Admin,Moder,Company")]
        public async Task<ActionResult<CompanyViewModel>> DeleteCompany(int id)
        {
            await _CompanyService.DeleteCompany(id);
            return Ok("Компания успешно удалена");
        }
        [HttpGet("GetOneCompany/{id}")]
        [AllowAnonymous]
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
                    model.CountryName = GetCountryName(Company.CountryId);
                    model.CityId = Company.CityId;
                    model.CityName =  GetCityName(Company.CityId);
                    model.Phone = Company.Phone;
                    model.Email = Company.Email;
                    model.CategoryId = Company.CategoryId;
                    model.CategoryName =  GetCategoryName(Company.CategoryId);
                    return new ObjectResult(model);
                }
                return BadRequest("Компания не найдена");
            }
            return BadRequest();
        }
        
        [HttpGet("GetAllCompany")]
        [AllowAnonymous]
        public IEnumerable<CompanyViewModel> Index()
        {
            List<CompanyViewModel> model = new List<CompanyViewModel>();
            if (_CompanyService != null)
            {
                _CompanyService.GetAllCompany().ToList().ForEach( u =>
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
                        CategoryName =  GetCategoryName(u.CategoryId),
                        CityName =  GetCityName(u.CityId),
                        CountryName =  GetCountryName(u.CountryId),
                    };
                    model.Add(Company);
                });
            }
            return model;
        }
        [HttpGet("GetVacancy/{id}")]
        [Authorize(Roles = "Admin,Moder,Company")]
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
        [Authorize(Roles = "Admin,Moder,Company")]
        public async Task<IActionResult> CreateVacancy(VacancieViewModel model)
        {
            VacancieViewModel vacancy = new VacancieViewModel
            {
                CityId =  GetCityId(model.CityId),
                CountryId =  GetCountryId(model.CountryId),
                AboutWork = model.AboutWork.Trim(),
                WorkName = model.WorkName.Trim(),
                CompanyId = model.CompanyId,
                Salary = model.Salary,
            };
            if (model.CompanyName != null)
            {
                string answer = await _companyProducer.CreateVacancieForCompany(vacancy);
                return Ok(answer);
            }
            return BadRequest("Не все обязательные поля были заполнены");
        }
        [HttpPut("EditVacancy")]
        [Authorize(Roles = "Admin,Moder,Company")]
        public async Task<IActionResult> EditVacancy(VacancieViewModel model)
        {
            VacancieViewModel vacancy = new VacancieViewModel
            {
                CityId =  GetCityId(model.CityId),
                CountryId =  GetCountryId(model.CountryId),
                AboutWork = model.AboutWork.Trim(),
                WorkName = model.WorkName.Trim(),
                CompanyId = model.CompanyId,
                Salary = model.Salary,
                Id = model.Id
            };
            if (model.CompanyName != null)
            {
                string answer = await _companyProducer.EditVacancieForCompany(vacancy);
                return Ok(answer);
            }
            return BadRequest("Не все обязательные поля были заполнены");
        }
        [HttpGet("GetVacancyForCompany/{companyId}")]
        [Authorize(Roles = "Admin,Moder,Company")]
        public IEnumerable<VacancieViewModel> AllForCompanyVacancy(int companyId)
        {
            var model = _companyProducer.TakeAllVacanciesOfCompany(companyId);
            return model;
        }
        [HttpDelete("DeleteVacancy/{id}")]
        [Authorize(Roles = "Admin,Moder,Company")]
        public async Task<ActionResult<CompanyViewModel>> DeleteVacancy(int id)
        {
            await _companyProducer.DeleteVacancieForCompany(id);
            return Ok("Вакансия успешно удалена");
        }
        private int GetCategoryId(int id)
        {
            var category = _rpc.GetCategory(id);
            return (int)category.Id;
        }
        private string GetCategoryName(int id)
        {
            var model = _rpc.GetCategory(id);
            return model.CategoryName;
        }
        private int GetCityId(int id)
        {
            var city = _Locrpc.GetCityById(id);
            return (int)city.Id;
        }
        private int GetCountryId(int id)
        {
            var country = _Locrpc.GetCountryById(id);
            return (int)country.Id;
        }
        private string GetCountryName(int id)
        {
            var country = _Locrpc.GetCountryById(id);
            return country.CountryName;
        }
        private string GetCityName(int id)
        {
            var city = _Locrpc.GetCityById(id);
            return city.CityName;
        }
    }
}
