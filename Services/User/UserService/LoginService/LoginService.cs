using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserDomain.Models;
using UserRepository.UserContext;
using UserRepository.UserLogic;

namespace UserService.LoginService
{
    public class LoginService : ILoginService
    {
        private IUserLogic<UserModel> userControlLogic;
        private UserDbContext _context;
        public LoginService(IUserLogic<UserModel> userControl, UserDbContext context)
        {
            this.userControlLogic = userControl;
            _context = context;
        }

        public async Task<UserModel> GetUser(string name, string password)
        {
            UserModel? user = _context.User.Include(u => u.Role).FirstOrDefault(x => x.Email == name && x.Password == password);
            if (user != null)
            {
                return await userControlLogic.Get((int)user.Id);
            }
            else
            {
                throw new ArgumentException($"Пользователя с именем {name}, не найдено");
            }
        }
    }
}
