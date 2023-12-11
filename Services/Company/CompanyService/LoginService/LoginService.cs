using CompanyDomain.Model;
using CompanyRepository;
using CompanyRepository.CompanyLogic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserDomain.Models;

namespace CompanyService.LoginService
{
    public class LoginService : ILoginService
    {
        private ICompanyLogic<CompanyModel> _companyControlLogic;
        private CompanyContext _context;
        public LoginService(ICompanyLogic<CompanyModel> companyControlLogic, CompanyContext context)
        {
            this._companyControlLogic= companyControlLogic;
            _context = context;
        }

        public async Task<CompanyModel> GetUserCompany(string name, string password)
        {
            CompanyModel? company = _context.Companie.Include(u => u.Role).FirstOrDefault(x => x.Email == name && x.Password == password);
            if (company != null)
            {
                return await _companyControlLogic.GetCompany((int)company.Id);
            }
            else
            {
                throw new ArgumentException($"Пользователя с именем {name}, не найдено");
            }
        }
    }
}
