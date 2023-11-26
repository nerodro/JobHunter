using CompanyDomain.Model;
using CompanyRepository.CompanyLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyService.CompanyService
{
    public class CompanyServices : ICompanyService
    {
        private ICompanyLogic<CompanyModel> _Company;
        public CompanyServices(ICompanyLogic<CompanyModel> Company)
        {
            _Company = Company;
        }
        public IEnumerable<CompanyModel> GetAllCompany()
        {
            return _Company.GetAllCompanies();
        }
        public async Task<CompanyModel> GetCompany(int id)
        {
            return await _Company.GetCompany(id);
        }
        public async Task CreateCompany(CompanyModel Company)
        {
            await _Company.CreateCompany(Company);
        }
        public async Task UpdateCompany(CompanyModel Company)
        {
            await _Company.UpdateCompany(Company);
        }
        public async Task DeleteCompany(int id)
        {
            CompanyModel Company = await GetCompany(id);
            await _Company.DeleteCompany(Company);
            await _Company.SaveChanges();
        }
    }
}
