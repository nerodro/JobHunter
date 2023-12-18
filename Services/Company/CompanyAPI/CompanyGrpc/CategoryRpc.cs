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
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            var channel = GrpcChannel.ForAddress("https://categoryapi:443",
                new GrpcChannelOptions { HttpHandler = handler });
            //var channel = GrpcChannel.ForAddress("https://localhost:7236");
            //var channel = GrpcChannel.ForAddress("https://categoryapi:443");
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
