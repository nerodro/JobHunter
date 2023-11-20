using Grpc.Core;
using Grpc.Net.Client;
using System.Threading.Channels;
using UserAPI.ViewModel;

namespace UserAPI.ServiceGrpc
{
    public class CategoryRpc
    {
        private readonly CategoryServiceGrpc.CategoryServiceGrpcClient _rpc;
        public CategoryRpc()
        {
            var channel = GrpcChannel.ForAddress("https://localhost:7236");
            _rpc = new CategoryServiceGrpc.CategoryServiceGrpcClient(channel);
        }
        public int GetCategoryById(int categoryId)
        {
            var request = new CategoryRequestGrpc
            {
                CategoryId = categoryId
            };

            var response = _rpc.GetCategoryById(request);
            return (int)response.Category.CategoryId;
        }
        public async Task<CategoryViewModel> GetCategoryModel(int categoryId)
        {
            var request = new CategoryRequestGrpc
            {
                CategoryId = categoryId
            };

            var response = _rpc.GetCategoryById(request);
            CategoryViewModel model = new CategoryViewModel
            {
                Id = response.Category.CategoryId,
                CategoryName = response.Category.CategoryName
            };
            return model;
        }
    }
}
