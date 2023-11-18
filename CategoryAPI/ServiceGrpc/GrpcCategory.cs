using CategoryAPI.ViewModel;
using CategoryDomain.Model;
using CategoryService.CategoryService;
using Grpc.Core;
using static CategoryServiceGrpc;

namespace CategoryAPI.ServiceGrpc
{
    public class GrpcCategory : CategoryServiceGrpcBase
    {
        private readonly ICategoryService _categoryService;
        public GrpcCategory(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        public async override Task<CategoryResponseGrpc> GetCategoryById(CategoryRequestGrpc request, ServerCallContext context)
        {
            // Ваша логика для получения категории по идентификатору
            int categoryId = request.CategoryId;
            CategoryModel model = await _categoryService.GetCategory(categoryId);

            CategoryGrpc category = new CategoryGrpc
            {
                CategoryId = model.Id,
                CategoryName = model.CategoryName
            };

            var response = new CategoryResponseGrpc
            {
                Category = category
            };

            return await Task.FromResult(response);
        }
    }
}
