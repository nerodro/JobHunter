using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserDomain.Models;

namespace UserService.LoginService
{
    public interface ILoginService
    {
        Task<UserModel> GetUser(string name, string password);
    }
}
