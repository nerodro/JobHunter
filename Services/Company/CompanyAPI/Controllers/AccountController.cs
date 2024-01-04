using Microsoft.AspNetCore.Authentication.Cookies;
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

namespace UserAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IRegistrationService _registeredServices;
        private readonly ILoginService _loginService;
        private readonly LocationRpc _rpc;
        public AccountController(IRegistrationService registered, ILoginService login, LocationRpc rpc)
        {
            this._registeredServices = registered;
            this._loginService = login;
            _rpc = rpc;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register(CompanyViewModel model)
        {
            CompanyModel companyEntity = new CompanyModel
            {
                Email = model.Email.Trim(),
                CompanyName = model.CompanyName.Trim(),
                Phone = model.Phone,
                CityId = await GetCityId(model.CityId),
                CountryId = await GetCountryId(model.CountryId),
                RoleId = model.RoleId,
                Password = HasPassword(model.Password),
            };
        await _registeredServices.CreateUserForCompany(companyEntity);

            if (companyEntity.Id > 0)
            {
                await Authenticate(companyEntity);
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
                CompanyModel companyEntity = await _loginService.GetUserCompany(name, password);

                if (companyEntity != null)
                {
                    await Authenticate(companyEntity);
                    return Ok(model);
                }
                else if (companyEntity == null)
                {
                    ModelState.AddModelError("Ошибка", "Ошибка");
                }
            }
            return BadRequest();
        }
        private async Task Authenticate(CompanyModel user)
        {
            // создаем один claim
            if (user != null)
            {
                var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.RoleName),
            };
                // создаем объект ClaimsIdentity
                ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                // установка аутентификационных куки
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
            }

        }
        [HttpGet("Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok();
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
