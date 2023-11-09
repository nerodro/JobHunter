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
        public void Create(T entity)
        {
           if(entity == null)
           {
                throw new ArgumentNullException("entity");
           }
           _dbSet.Add(entity);
           _userContext.SaveChanges();
        }

        public void Delete(T entity)
        {
            if(entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _dbSet.Remove(entity);
            _userContext.SaveChanges();
        }

        public  T Get(long id)
        {
            if(id != 0) 
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

    }
}
