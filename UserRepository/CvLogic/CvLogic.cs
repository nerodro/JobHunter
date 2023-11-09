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
        public void Create(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _dbSet.Add(entity);
            _userContext.SaveChanges();
        }

        public void Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _dbSet.Remove(entity);
            _userContext.SaveChanges();
        }

        public T Get(long id)
        {
            if (id != 0)
            {
                throw new ArgumentNullException("entity");
            }
            return _dbSet.SingleOrDefault(x => x.Id == id);
        }

        public void Remove(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _dbSet.Remove(entity);
        }

        public void SaveChanges()
        {
            _userContext.SaveChanges();
        }

        public void Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _userContext.SaveChanges();
        }
        public IEnumerable<T> GetAll()
        {
            return _dbSet.AsEnumerable();
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
