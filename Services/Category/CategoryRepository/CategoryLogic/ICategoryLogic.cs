using CategoryDomain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CategoryRepository.CategoryLogic
{
    public interface ICategoryLogic<T> where T : CategoryModel
    {
        IEnumerable<T> GetAll();
        Task<T> Get(int id);
        Task Create(T entity);
        Task Update(T entity);
        Task Delete(T entity);
        void Remove(T entity);
        Task SaveChanges();
    }
}
