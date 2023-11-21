using LocationDomain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocationRepository.CountryLogic
{
    public class CountryLogic<T> : ICountryLogiccs<T> where T : CountryModel
    {
        private readonly LocationContext _locationContext;
        private DbSet<T> _dbSet;

        public CountryLogic(LocationContext context)
        {
            this._locationContext = context;
            _dbSet = context.Set<T>();
        }
        public async Task CreateCountry(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            await _dbSet.AddAsync(entity);
            await _locationContext.SaveChangesAsync();
        }

        public async Task DeleteCountry(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _dbSet.Remove(entity);
            await _locationContext.SaveChangesAsync();
        }

        public async Task<T> GetCountry(int id)
        {
            if (id == 0)
            {
                throw new ArgumentNullException("entity");
            }
            var Country = await _dbSet.SingleOrDefaultAsync(x => x.Id == id);
            if (Country == null)
            {
                throw new ArgumentException($"Страны с Id {id}, не найдено");
            }
            return Country;

        }

        public void RemoveCountry(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _dbSet.Remove(entity);
        }

        public async Task SaveChanges()
        {
            await _locationContext.SaveChangesAsync();
        }

        public async Task UpdateCountry(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            await _locationContext.SaveChangesAsync();
        }
        public IEnumerable<T> GetAllCountry()
        {
            return _dbSet.AsEnumerable();
        }
    }
}
