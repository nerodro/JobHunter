using Grpc.Net.Client;
using UserAPI.ViewModel;

namespace VacancieAPI.VacancieRpc
{
    public class UserRpc
    {
        private readonly UserServiceGrpc.UserServiceGrpcClient _rpc;
        public UserRpc()
        {
            var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .AddEnvironmentVariables()
                    .Build();

            var grpcConnection = configuration.GetSection("Grpc:CvHttp").Value!;
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            var channel = GrpcChannel.ForAddress(grpcConnection,
                new GrpcChannelOptions { HttpHandler = handler });
            _rpc = new UserServiceGrpc.UserServiceGrpcClient(channel);
        }
        public async Task<UserViewModel> GetUser(int UserId)
        {
            var request = new UserRequestGrpc
            {
                UserId = UserId
            };

            var response = _rpc.GetUserById(request);
            UserViewModel User = new UserViewModel
            {
                Id = response.User.UserId,
                Name = response.User.UserName
            };
            return User;
        }
    }
}
