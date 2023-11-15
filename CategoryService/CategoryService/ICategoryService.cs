using CategoryDomain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CategoryService.CategoryService
{
    public interface ICategoryService
    {
        IEnumerable<CategoryModel> GetAll();
        Task<CategoryModel> GetCategory(int id);
        Task Create(CategoryModel model);
        Task Update(CategoryModel model);
        Task Delete(int id);
    }
}
