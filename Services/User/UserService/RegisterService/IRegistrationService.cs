using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserDomain.Models;

namespace UserService.RegistrationService
{
    public interface IRegistrationService
    {
        Task CreateUser(UserModel user);
    }
}
