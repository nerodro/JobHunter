using CompanyDomain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyRepository.CompanyLogic
{
    public class CompanyLogic<T> : ICompanyLogic<T> where T : CompanyModel
    {
        private readonly CompanyContext _companyContext;
        private DbSet<T> _dbSet;
        public CompanyLogic(CompanyContext context)
        {
            this._companyContext = context;
            _dbSet = context.Set<T>();
        }
        public async Task CreateCompany(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            await _dbSet.AddAsync(entity);
            await _companyContext.SaveChangesAsync();
        }

        public async Task DeleteCompany(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _dbSet.Remove(entity);
            await _companyContext.SaveChangesAsync();
        }

        public async Task<T> GetCompany(int id)
        {
            if (id == 0)
            {
                throw new ArgumentNullException("entity");
            }
            var user = await _dbSet.SingleOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                throw new ArgumentException($"Компания с Id {id}, не найдено");
            }
            return user;
        }

        public void RemoveCompany(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _dbSet.Remove(entity);
        }

        public async Task SaveChanges()
        {
            await _companyContext.SaveChangesAsync();
        }

        public async Task UpdateCompany(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            await _companyContext.SaveChangesAsync();
        }
        public IEnumerable<T> GetAllCompanies()
        {
            return _dbSet.AsEnumerable().OrderByDescending(x => x.Id);
        }
    }
}
