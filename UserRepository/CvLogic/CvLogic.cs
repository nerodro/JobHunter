using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserDomain.Models;
using UserRepository.UserContext;
using UserRepository.UserLogic;

namespace UserRepository.CvLogic
{
    public class CvLogic<T> : ICvLogic<T> where T : CvModel
    {
        private readonly UserDbContext _userContext;
        private DbSet<T> _dbSet;

        public CvLogic(UserDbContext context)
        {
            this._userContext = context;
            _dbSet = context.Set<T>();
        }
        public async Task Create(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            await _dbSet.AddAsync(entity);
            await _userContext.SaveChangesAsync();
        }

        public async Task Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _dbSet.Remove(entity);
            await _userContext.SaveChangesAsync();
        }

        public async Task<T> Get(int id)
        {
            if (id == 0)
            {
                throw new ArgumentNullException("entity");
            }
            var cv = await _dbSet.SingleOrDefaultAsync(x => x.Id == id);
            if (cv == null)
            {
                throw new ArgumentException($"Резюме с Id {id}, не найдено");
            }
            return cv;
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
            await _userContext.SaveChangesAsync();
        }

        public async Task Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            await _userContext.SaveChangesAsync();
        }
        public IEnumerable<T> GetAll()
        {
            return _dbSet.AsEnumerable();
        }
    }
}
