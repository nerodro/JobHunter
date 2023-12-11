using System;
using CompanyDomain.Model;
using CompanyRepository;
using Microsoft.EntityFrameworkCore;
using UserDomain.Models;

namespace CompanyRepository.Registration
{
    public class RegistrationLogic<T> : IRegistration<T> where T : CompanyModel
    {
        private readonly CompanyContext context;
        private DbSet<T> entities;
        string errorMessage = string.Empty;
        public RegistrationLogic(CompanyContext context)
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
