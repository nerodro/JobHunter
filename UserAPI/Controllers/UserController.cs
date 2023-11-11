using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
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
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser(UserViewModel model)
        {
            UserModel user = new UserModel
            {
                Email = model.Email,
                Name = model.Name,
                Patronomyc = model.Patronomyc,
                Phone = model.Phone,
                CityId = model.CityId,
                CountryId = model.CountryId,
                Surname = model.Surname,
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
                userEntity.Email = model.Email;
                userEntity.Name = model.Name;
                userEntity.Surname = model.Surname;
                userEntity.Phone = model.Phone;
                userEntity.Patronomyc = model.Patronomyc;
                userEntity.CityId = model.CityId;
                userEntity.CountryId = model.CountryId;
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
                model.CountryId = userEntity.CountryId;
                model.RoleId = userEntity.RoleId;
                model.Email = userEntity.Email;
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
                _userService.GetAll().ToList().ForEach(u =>
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
                        Patronomyc = u.Patronomyc
                    };
                    model.Add(user);
                });
            }
            return model;
        }
    }
}
