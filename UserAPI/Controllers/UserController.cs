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
        public async Task<ActionResult<UserViewModel>> CreateUser(UserViewModel model)
        {
            UserModel user = new UserModel
            {
                Email = model.Email,
                Name = model.Name,
                Password = model.Password,
                Patronomyc = model.Patronomyc,
                Phone = model.Phone,
                CityId = model.CityId,
                CountryId = model.CountryId,
                Surname = model.Surname,
                RoleId = model.RoleId,
            };
            if(model.Email != null && model.Name != null) 
            {
                await _userService.Create(user);
                return CreatedAtAction("SingleUser", new { id = user.Id }, model);
            }
            return BadRequest("Не все обязательные поля были заполнены");
        }
    }
}
