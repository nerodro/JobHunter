﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using CompanyService.RegistrationService;
using CompanyService.LoginService;
using CompanyAPI.ServiceGrpc;
using CompanyAPI.ViewModel;
using CompanyDomain.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace UserAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IRegistrationService _registeredServices;
        private readonly ILoginService _loginService;
        private readonly LocationRpc _rpc;
        private readonly IConfiguration _configuration;
        public AccountController(IRegistrationService registered, ILoginService login, LocationRpc rpc, IConfiguration configuration)
        {
            this._registeredServices = registered;
            this._loginService = login;
            _rpc = rpc;
            _configuration = configuration;
        }
        [HttpPost("RegisterCompany")]
        public async Task<IActionResult> Register(CompanyViewModel model)
        {
            CompanyModel companyEntity = new CompanyModel
            {
                Email = model.Email.Trim(),
                CompanyName = model.CompanyName.Trim(),
                Phone = model.Phone,
                CityId =  GetCityId(model.CityId),
                CountryId =  GetCountryId(model.CountryId),
                RoleId = 3,
                Password = HasPassword(model.Password),
                CategoryId = model.CategoryId
            };
            await _registeredServices.CreateUserForCompany(companyEntity);

            if (companyEntity.Id > 0)
            {
                return Ok(model);
            }
            else
            {
                ModelState.AddModelError("", "Ошибка");
            }
            return BadRequest();
        }

        [HttpPost("LoginCompany")]
        public async Task<ActionResult> LoginAsync(LoginViewModel login)
        {
            if (ModelState.IsValid)
            {
                login.Password = HasPassword(login.Password);
                CompanyModel companyEntity = await _loginService.GetUserCompany(login.FirstName, login.Password);

                if (companyEntity != null)
                {
                    string token = JwtToken(companyEntity);
                    return Ok(token);
                }
                else if (companyEntity == null)
                {
                    ModelState.AddModelError("Ошибка", "Ошибка");
                }
            }
            return BadRequest();
        }
        private string JwtToken(CompanyModel company)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, company.CompanyName),
                new Claim(ClaimTypes.Role, company.Role.RoleName)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("Jwt:Token").Value!));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var jwt = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddYears(1),
                signingCredentials: cred
                );
            var token = new JwtSecurityTokenHandler().WriteToken(jwt);
            return token;
        }
        private static string HasPassword(string Password)
        {
            MD5 md5 =  MD5.Create();
            byte[] b = Encoding.ASCII.GetBytes(Password);
            byte[] hash =  md5.ComputeHash(b);
            StringBuilder sb = new StringBuilder();
            foreach (var i in hash)
            {
                sb.Append(i.ToString("X2"));
            }
            return Convert.ToString(sb)!;
        }
        private int GetCityId(int id)
        {
            var city = _rpc.GetCityById(id);
            return (int)city.Id;
        }
        private int GetCountryId(int id)
        {
            var country = _rpc.GetCountryById(id);
            return (int)country.Id;
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
    }
}
