using CompanyDomain.Model;
using CompanyRepository;
using CompanyRepository.CompanyLogic;
using CompanyRepository.Registration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using UserDomain.Models;

namespace CompanyService.RegistrationService
{
    public class RegistrationService : IRegistrationService
    {
        private readonly CompanyContext _context;
        private readonly IRegistration<CompanyModel> _registrationService;
        private readonly ICompanyLogic<CompanyModel> _CompanyLogic;
        public RegistrationService( IRegistration<CompanyModel> registration, ICompanyLogic<CompanyModel> CompanyLogic, CompanyContext context) 
        {
            this._registrationService = registration;
            this._CompanyLogic = CompanyLogic;
            this._context = context;
        }
        public async Task CreateUserForCompany(CompanyModel CompanyModel)
        {
            //CompanyModel Company1 = _context.Company.FirstOrDefault(x => x.Email == CompanyModel.Email && x.Password == CompanyModel.Password);
            var Company = _CompanyLogic.GetAllCompanies().FirstOrDefault(x => x.CompanyName == CompanyModel.CompanyName && x.Password == CompanyModel.Password);
            RoleModel? CompanyRole = _context.Role.FirstOrDefault(r => r.RoleName == "Company");
            if (CompanyRole != null)
            {
                CompanyModel.Role = CompanyRole;
            }
            if (Company == null)
            {
                await _registrationService.Create(CompanyModel);
            }

        }
    }
}
