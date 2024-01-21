using Grpc.Core;
using Grpc.Net.Client;
using System.Threading.Channels;
using CompanyAPI.ViewModel;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CompanyAPI.ServiceGrpc
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

            var grpcConnection = configuration.GetSection("Grpc:CategoryHttp").Value!;
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            var channel = GrpcChannel.ForAddress(grpcConnection,
                new GrpcChannelOptions { HttpHandler = handler });
            _rpc = new CategoryServiceGrpc.CategoryServiceGrpcClient(channel);
        }
        public CategoryViewModel GetCategory(int categoryId)
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
