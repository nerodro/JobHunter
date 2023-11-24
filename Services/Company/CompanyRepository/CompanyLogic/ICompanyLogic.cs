using CompanyDomain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyRepository.CompanyLogic
{
    public interface ICompanyLogic<T> where T : CompanyModel
    {
        IEnumerable<T> GetAllCompanies();
        Task<T> GetCompany(int id);
        Task CreateCompany(T entity);
        Task UpdateCompany(T entity);
        Task DeleteCompany(T entity);
        void RemoveCompany(T entity);
        Task SaveChanges();
    }
}
