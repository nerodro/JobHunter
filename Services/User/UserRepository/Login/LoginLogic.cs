using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserDomain.Models;
using UserRepository.UserContext;

namespace UserRepository.Login
{
    public class LoginLogic<T> : ILogin<T> where T : UserModel
    {
        private readonly UserDbContext _dbcontext;
        private DbSet<T> entities;
        public LoginLogic(UserDbContext context)
        {
            _dbcontext = context;
            entities = context.Set<T>();
        }
        public async Task<T> Get(long id)
        {
            return entities.SingleOrDefault(s => s.Id == id);
            //if (id == 0)
            //{
            //    throw new ArgumentNullException("entity");
            //}
            //var user = await entities.FirstOrDefaultAsync(x => x.Id == id);
            //if (user == null)
            //{
            //    throw new ArgumentException($"Пользователя с Id {id}, не найдено");
            //}
            //return user;
        }
    }
}
