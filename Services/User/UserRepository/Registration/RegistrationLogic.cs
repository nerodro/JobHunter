using System;
using Microsoft.EntityFrameworkCore;
using UserDomain.Models;
using UserRepository.UserContext;

namespace UserRepository.Registration
{
    public class RegistrationLogic<T> : IRegistration<T> where T : UserModel
    {
        private readonly UserDbContext context;
        private DbSet<T> entities;
        string errorMessage = string.Empty;
        public RegistrationLogic(UserDbContext context)
        {
            this.context = context;
            entities = context.Set<T>();
        }

        public async Task Create(T entity)
        {
        
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            await entities.AddAsync(entity);
            await context.SaveChangesAsync();
        }
    }
}
