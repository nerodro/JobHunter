using CategoryAPI.ViewModel;
using CategoryDomain.Model;
using CategoryService.CategoryService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CategoryAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [HttpPost("CreateCategory")]
        [Authorize(Roles = "Admin,Moder")]
        public async Task<IActionResult> CreateCategory(CategoryViewModel model)
        {
            CategoryModel language = new CategoryModel
            {
                CategoryName = model.CategoryName.Trim()
            };
            if (model.CategoryName != null)
            {
                await _categoryService.Create(language);
                return CreatedAtAction("SingleCategory", new { id = language.Id }, model);
            }
            return BadRequest("Не все обязательные поля были заполнены");
        }
        [HttpPut("EditCategory/{id}")]
        [Authorize(Roles = "Admin,Moder")]
        public async Task<IActionResult> EditCategory(int id, CategoryViewModel model)
        {
            CategoryModel category = await _categoryService.GetCategory(id);
            if (ModelState.IsValid)
            {
                category.CategoryName = model.CategoryName.Trim();
                if (model.CategoryName != null)
                {
                    await _categoryService.Update(category);
                    return Ok(model);
                }
            }
            return BadRequest(ModelState);
        }
        [HttpDelete("DeleteCategory/{id}")]
        [Authorize(Roles = "Admin,Moder")]
        public async Task<ActionResult<CategoryViewModel>> DeleteCategory(int id)
        {
            var model = await _categoryService.GetCategory(id);
            if (model != null)
            {
                await _categoryService.Delete(id);
                return Ok("Категория успешно удалена");
            }
            return BadRequest();
        }
        [HttpGet("GetOneCategory/{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<CategoryViewModel>> SingleCategory(int id)
        {
            CategoryViewModel model = new CategoryViewModel();
            if (id != 0)
            {
                CategoryModel category = await _categoryService.GetCategory(id);
                if (category != null)
                {
                    model.CategoryName = category.CategoryName;
                    model.Id = category.Id;
                    return new ObjectResult(model);
                }
                return BadRequest("Категория не найдена");
            }
            return BadRequest();
        }
        [HttpGet("GetAllCategory")]
        [AllowAnonymous]
        public IEnumerable<CategoryViewModel> Index()
        {
            List<CategoryViewModel> model = new List<CategoryViewModel>();
            if (_categoryService != null)
            {
                _categoryService.GetAll().ToList().ForEach(u =>
                {
                    CategoryViewModel category = new CategoryViewModel
                    {
                        Id = u.Id,
                        CategoryName = u.CategoryName
                    };
                    model.Add(category);
                });
            }
            return model;
        }
    }
}
