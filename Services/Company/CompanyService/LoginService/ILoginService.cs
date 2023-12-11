using CompanyDomain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserDomain.Models;

namespace CompanyService.LoginService
{
    public interface ILoginService
    {
        Task<CompanyModel> GetUserCompany(string name, string password);
    }
}
