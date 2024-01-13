using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacancieDomain.Model;

namespace VacancieRepository.VacancieLogic
{
    public class VacancieLogic<T> : IVacancieLogic<T> where T : VacancieModel
    {
        private readonly VacancyContext _vacancyContext;
        private DbSet<T> _dbSet;
        public VacancieLogic(VacancyContext context)
        {
            this._vacancyContext = context;
            _dbSet = context.Set<T>();
        }
        public async Task Create(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            await _dbSet.AddAsync(entity);
            await _vacancyContext.SaveChangesAsync();
        }

        public async Task Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _dbSet.Remove(entity);
            await _vacancyContext.SaveChangesAsync();
        }

        public async Task<T> Get(int id)
        {
            if (id == 0)
            {
                throw new ArgumentNullException("entity");
            }
            var vacancy = await _dbSet.SingleOrDefaultAsync(x => x.Id == id);
            if (vacancy == null)
            {
                throw new ArgumentException($"Вакансий с Id {id}, не найдено");
            }
            return vacancy;
        }

        public void Remove(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _dbSet.Remove(entity);
        }

        public async Task SaveChanges()
        {
            await _vacancyContext.SaveChangesAsync();
        }

        public async Task Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            await _vacancyContext.SaveChangesAsync();
        }
        public IEnumerable<T> GetAll()
        {
            return _dbSet.AsEnumerable().OrderByDescending(x => x.Id);
        }
    }
}
