using CategoryDomain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CategoryRepository.CategoryLogic
{
    public class CategoryLogic<T> : ICategoryLogic<T> where T :  CategoryModel
    {
        private readonly CategoryContext _categoryContext;
        private DbSet<T> _dbSet;
        public CategoryLogic(CategoryContext context)
        {
            this._categoryContext = context;
            _dbSet = context.Set<T>();
        }
        public async Task Create(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            await _dbSet.AddAsync(entity);
            await _categoryContext.SaveChangesAsync();
        }

        public async Task Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _dbSet.Remove(entity);
            await _categoryContext.SaveChangesAsync();
        }

        public async Task<T> Get(int id)
        {
            if (id == 0)
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
            await _categoryContext.SaveChangesAsync();
        }

        public async Task Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            await _categoryContext.SaveChangesAsync();
        }
        public IEnumerable<T> GetAll()
        {
            return _dbSet.AsEnumerable();
        }
    }
}
