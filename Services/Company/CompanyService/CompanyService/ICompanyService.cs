using CompanyDomain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyService.CompanyService
{
    public interface ICompanyService
    {
        IEnumerable<CompanyModel> GetAllCompany();
        Task<CompanyModel> GetCompany(int id);
        Task CreateCompany(CompanyModel model);
        Task UpdateCompany(CompanyModel model);
        Task DeleteCompany(int id);
    }
}
