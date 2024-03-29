﻿using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UserAPI.ServiceGrpc;
using UserAPI.ViewModel;
using UserDomain.Models;
using UserService.UserService;

namespace UserAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly LocationRpc _rpc;
        public UserController(IUserService userService, LocationRpc rpc)
        {
            _userService = userService;
            _rpc = rpc;
        }
        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser(UserViewModel model)
        {
            UserModel user = new UserModel
            {
                Email = model.Email.Trim(),
                Name = model.Name.Trim(),
                Patronomyc = model.Patronomyc.Trim(),
                Phone = model.Phone,
                CityId =  GetCityId(model.CityId),
                CountryId =  GetCountryId(model.CountryId),
                Surname = model.Surname.Trim(),
                RoleId = model.RoleId,
                Password = model.Password,
            };
            if (model.Email != null && model.Name != null)
            {
                await _userService.Create(user);
                return CreatedAtAction("UserProfile", new { id = user.Id }, model);
            }
            return BadRequest("Не все обязательные поля были заполнены");
        }
        [HttpPut("EditUser/{id}")]
        public async Task<ActionResult<UserViewModel>> EditUser(int id, UserViewModel model)
        {
            UserModel userEntity = await _userService.GetUser(id);
            if (ModelState.IsValid)
            {
                userEntity.Email = model.Email.Trim();
                userEntity.Name = model.Name.Trim();
                userEntity.Surname = model.Surname.Trim();
                userEntity.Phone = model.Phone;
                userEntity.Patronomyc = model.Patronomyc.Trim();
                userEntity.CityId =  GetCityId(model.CityId);
                userEntity.CountryId = GetCountryId(model.CountryId);
                userEntity.RoleId = model.RoleId;
                if (model.Email != null && model.Name != null)
                {
                    await _userService.Update(userEntity);
                    return Ok(model);
                }
            }
            return BadRequest(ModelState);
        }
        [HttpDelete("DeleteUser/{id}")]
        public async Task<ActionResult<UserViewModel>> DeleteUser(int id)
        {
            await _userService.Delete(id);
            return Ok("Пользователь успешно удален");
        }
        [HttpGet("GetOneUser/{id}")]
        public async Task<ActionResult<UserViewModel>> UserProfile(int id)
        {
            UserViewModel model = new UserViewModel();
            if (id != 0)
            {
                UserModel userEntity = await _userService.GetUser(id);
                model.Id = userEntity.Id;
                model.Name = userEntity.Name;
                model.Surname = userEntity.Surname;
                model.Phone = userEntity.Phone;
                model.CityId = userEntity.CityId;
                model.CityName =  GetCityName(model.CityId);
                model.CountryId = userEntity.CountryId;
                model.CountryName =  GetCountryName(model.CountryId);
                model.RoleId = userEntity.RoleId;
                model.Email = userEntity.Email;
                model.Patronomyc = userEntity.Patronomyc;
                return new ObjectResult(model);
            }
            return BadRequest();
        }
        [HttpGet("GetAllUsers")]
        public IEnumerable<UserViewModel> Index()
        {
            List<UserViewModel> model = new List<UserViewModel>();
            if (_userService != null)
            {
                _userService.GetAll().ToList().ForEach( u =>
                {
                    UserViewModel user = new UserViewModel
                    {
                        Id = u.Id,
                        Name = u.Name,
                        Surname = u.Surname,
                        Phone = u.Phone,
                        CityId = u.CityId,
                        CountryId = u.CountryId,
                        RoleId = u.RoleId,
                        Email = u.Email,
                        Patronomyc = u.Patronomyc,
                        CountryName =  GetCountryName(u.CountryId),
                        CityName =  GetCityName(u.CityId),
                    };
                    model.Add(user);
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
