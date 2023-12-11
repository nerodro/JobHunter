using CompanyDomain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserDomain.Models;

namespace CompanyService.RegistrationService
{
    public interface IRegistrationService
    {
        Task CreateUserForCompany(CompanyModel user);
    }
}
