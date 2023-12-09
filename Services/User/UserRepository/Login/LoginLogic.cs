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
        private readonly UserDbContext context;
        private DbSet<T> entities;
        string errorMessage = string.Empty;
        public LoginLogic(UserDbContext context)
        {
            this.context = context;
            entities = context.Set<T>();
        }
        public T Get(long id)
        {
            return entities.SingleOrDefault(s => s.Id == id);
        }
    }
}
