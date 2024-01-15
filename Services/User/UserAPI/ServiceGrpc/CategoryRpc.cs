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
            var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .AddEnvironmentVariables()
                    .Build();

            //var grpcConnection = configuration.GetSection("Grpc:CategoryHttp").Value!;
            var grpcConnection = "https://localhost:7236";
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            var channel = GrpcChannel.ForAddress(grpcConnection,
                new GrpcChannelOptions { HttpHandler = handler });
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
