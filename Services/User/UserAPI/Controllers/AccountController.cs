﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using UserService.RegistrationService;
using UserService.LoginService;
using UserDomain.Models;
using UserAPI.ViewModel;
using UserAPI.ServiceGrpc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Newtonsoft.Json.Linq;

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
        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserViewModel model)
        {
            UserModel userEntity = new UserModel
            {
                Email = model.Email.Trim(),
                Name = model.Name.Trim(),
                Patronomyc = model.Patronomyc.Trim(),
                Phone = model.Phone,
                CityId = await GetCityId(model.CityId),
                CountryId = await GetCountryId(model.CountryId),
                Surname = model.Surname.Trim(),
                RoleId = model.RoleId,
                Password = HasPassword(model.Password),
            };
            await _registeredServices.CreateUser(userEntity);

            if (userEntity.Id > 0)
            {
                return Ok(model);
            }
            else
            {
                ModelState.AddModelError("", "Ошибка");
            }
            return BadRequest();
        }

        [HttpGet("LoginUser/{name},{password}")]
        public async Task<ActionResult> LoginAsync(string name, string password)
        {
            LoginViewModel model = new LoginViewModel();
            if (ModelState.IsValid)
            {
                model.FirstName = name;
                model.Password = password;
                password = HasPassword(password);
                UserModel userEntity = await _loginService.GetUser(name, password);

                if (userEntity != null)
                {
                    string token = JwtToken(userEntity);
                    return Ok(token);
                }
                else if (userEntity == null)
                {
                    ModelState.AddModelError("Ошибка", "Ошибка");
                }
            }
            return BadRequest();
        }
        private string JwtToken(UserModel user) 
        {
            List <Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role.RoleName)
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
    }
}
