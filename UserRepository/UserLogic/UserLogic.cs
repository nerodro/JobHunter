using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            throw new NotImplementedException();
        }

        public T Get(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Remove(T entity)
        {
            throw new NotImplementedException();
        }

        public void SaveChanges()
        {
            throw new NotImplementedException();
        }

        public void Update(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
