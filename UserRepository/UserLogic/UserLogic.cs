using Microsoft.EntityFrameworkCore;
using UserDomain.Models;
using UserRepository.UserDbContext;

namespace UserRepository.UserLogic
{
    public class UserLogic<T> : IUserLogic<T> where T : UserModel
    {
        private readonly UserContext _userContext;
        private DbSet<T> _dbSet;
        public UserLogic(UserContext context)
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
            if(entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _dbSet.Remove(entity);
            await _userContext.SaveChangesAsync();
        }

        public async Task<T> Get(long id)
        {
            if(id != 0) 
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
        public IEnumerable<T> GetAll()
        {
            return _dbSet.AsEnumerable();
        }

    }
}
