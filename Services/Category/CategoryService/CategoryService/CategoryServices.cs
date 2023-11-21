using CategoryDomain.Model;
using CategoryRepository.CategoryLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CategoryService.CategoryService
{
    public class CategoryServices : ICategoryService
    {
        private readonly ICategoryLogic<CategoryModel> _categoryService;
        public CategoryServices(ICategoryLogic<CategoryModel> categoryService)
        {
            _categoryService = categoryService;
        }
        public IEnumerable<CategoryModel> GetAll()
        {
            return _categoryService.GetAll();
        }
        public async Task<CategoryModel> GetCategory(int id)
        {
            return await _categoryService.Get(id);
        }
        public async Task Create(CategoryModel model)
        {
            await _categoryService.Create(model);
        }
        public async Task Update(CategoryModel model)
        {
            await _categoryService.Update(model);
        }
        public async Task Delete(int id)
        {
            CategoryModel model = await _categoryService.Get(id);
            await _categoryService.Delete(model);
            await _categoryService.SaveChanges();
        }
    }
}
