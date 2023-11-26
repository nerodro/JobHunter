using Grpc.Core;
using Grpc.Net.Client;
using System.Threading.Channels;
using CompanyAPI.ViewModel;

namespace CompanyAPI.ServiceGrpc
{
    public class CategoryRpc
    {
        private readonly CategoryServiceGrpc.CategoryServiceGrpcClient _rpc;
        public CategoryRpc()
        {
            var channel = GrpcChannel.ForAddress("https://localhost:7236");
            _rpc = new CategoryServiceGrpc.CategoryServiceGrpcClient(channel);
        }
        public async Task<CategoryViewModel> GetCategory(int categoryId)
        {
            var request = new CategoryRequestGrpc
            {
                CategoryId = categoryId
            };

            var response = _rpc.GetCategoryById(request);
            CategoryViewModel category = new CategoryViewModel 
            {
                Id = response.Category.CategoryId,
                CategoryName = response.Category.CategoryName
            };
            return category;
        }
    }
}
