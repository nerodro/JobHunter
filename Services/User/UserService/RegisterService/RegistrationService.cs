using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserDomain.Models;
using UserRepository.Registration;
using UserRepository.UserContext;
using UserRepository.UserLogic;

namespace UserService.RegistrationService
{
    public class RegistrationService : IRegistrationService
    {
        private readonly UserDbContext _context;
        private readonly IRegistration<UserModel> _registrationService;
        private readonly IUserLogic<UserModel> _userLogic;
        public RegistrationService( IRegistration<UserModel> registration, IUserLogic<UserModel> userLogic, UserDbContext context) 
        {
            this._registrationService = registration;
            this._userLogic = userLogic;
            this._context = context;
        }
        public async Task CreateUser(UserModel user)
        {

            UserModel user1 = _context.User.FirstOrDefault(x => x.Email == user.Email && x.Password == user.Password);
            RoleModel userRole = _context.Role.FirstOrDefault(r => r.RoleName == "User");
            if (userRole != null)
            {
                user.Role = userRole;
            }
            if (user1 == null)
            {
                await _registrationService.Create(user);
            }

        }
    }
}
