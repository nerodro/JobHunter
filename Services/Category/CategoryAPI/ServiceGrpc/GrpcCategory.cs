using CategoryAPI.ViewModel;
using CategoryDomain.Model;
using CategoryService.CategoryService;
using Grpc.Core;
using Microsoft.AspNetCore.Http.HttpResults;
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
            int categoryId = request.CategoryId;
            CategoryModel model = await _categoryService.GetCategory(categoryId);

            if (model == null)
            {
                throw new RpcException(new Status(StatusCode.Cancelled, "Запрашиваемая категория не найдена"));
            }

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
