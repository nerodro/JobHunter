using LocationDomain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocationRepository.CitryLogic
{
    public class CityLogic<T> : ICityLogic<T> where T : CityModel
    {
        private readonly LocationContext _locationContext;
        private DbSet<T> _dbSet;

        public CityLogic(LocationContext context)
        {
            this._locationContext = context;
            _dbSet = context.Set<T>();
        }
        public async Task CreateCity(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            await _dbSet.AddAsync(entity);
            await _locationContext.SaveChangesAsync();
        }

        public async Task DeleteCity(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _dbSet.Remove(entity);
            await _locationContext.SaveChangesAsync();
        }

        public async Task<T> GetCity(int id)
        {
            if (id == 0)
            {
                throw new ArgumentNullException("entity");
            }
            var city = await _dbSet.SingleOrDefaultAsync(x => x.Id == id);
            if (city == null)
            {
                throw new ArgumentException($"Города с Id {id}, не найдено");
            }
            return city;
        }

        public void RemoveCity(T entity)
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

        public async Task UpdateCity(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            await _locationContext.SaveChangesAsync();
        }
        public IEnumerable<T> GetAllCity()
        {
            return _dbSet.AsEnumerable();
        }
    }
}
