using Microsoft.EntityFrameworkCore;
using UserDomain.Models;
using UserRepository.UserDbContext;

namespace UserRepository.CvLogic
{
    public class CvLogic<T> : ICvLogic<T> where T : CvModel
    {
        private readonly UserContext _userContext;
        private DbSet<T> _dbSet;
        public CvLogic(UserContext context)
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

        public async Task<T> Get(long id)
        {
            if (id != 0)
            {
                throw new ArgumentNullException("entity");
            }
            return await _dbSet.SingleOrDefaultAsync(x => x.Id == id);
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
        public IAsyncEnumerable<T> GetAll()
        {
            return _dbSet.AsAsyncEnumerable();
        }

        public void DeleteOfUser(List<T> entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            if (entity.Count() != 0)
            {
                _dbSet.RemoveRange(entity);
            }
        }
    }
}
