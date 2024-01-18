using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacancieDomain.Model;

namespace VacancieRepository.FavoriteLogic
{
    public class FavoriteLogic<T> : IFavoriteLogic<T> where T : FavoriteVacancie
    {
        private readonly VacancyContext _vacancyContext;
        private DbSet<T> _dbSet;
        public FavoriteLogic(VacancyContext context)
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
        public IEnumerable<T> GetAll()
        {
            return _dbSet.AsEnumerable().OrderByDescending(x => x.Id);
        }
        public IEnumerable<T> GetAllForUser(int Id)
        {
            return _dbSet.AsEnumerable().Where(x => x.UserId == Id).OrderByDescending(x => x.Id);
        }
    }
}
