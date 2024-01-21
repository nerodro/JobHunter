using Grpc.Net.Client;
using VacancieAPI.ViewModel;

namespace VacancieAPI.VacancieRpc
{
    public class CompanyRpc
    {
        private readonly CompanyServiceGrpc.CompanyServiceGrpcClient _rpc;
        public CompanyRpc()
        {
            var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .AddEnvironmentVariables()
                    .Build();

            var grpcConnection = configuration.GetSection("Grpc:CompanyHttp").Value!;
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            var channel = GrpcChannel.ForAddress(grpcConnection,
                new GrpcChannelOptions { HttpHandler = handler });
            _rpc = new CompanyServiceGrpc.CompanyServiceGrpcClient(channel);
        }
        public async Task<CompanyViewModel> GetCompany(int CompanyId)
        {
            var request = new CompanyRequestGrpc
            {
                CompanyId = CompanyId
            };

            var response = _rpc.GetCompanyById(request);
            CompanyViewModel Company = new CompanyViewModel
            {
                Id = response.Company.CompanyId,
                CompanyName = response.Company.CompanyName
            };
            return Company;
        }
        public CompanyViewModel GetCompanySync(int CompanyId)
        {
            var request = new CompanyRequestGrpc
            {
                CompanyId = CompanyId
            };

            var response = _rpc.GetCompanyById(request);
            CompanyViewModel Company = new CompanyViewModel
            {
                Id = response.Company.CompanyId,
                CompanyName = response.Company.CompanyName
            };
            return Company;
        }

    }
}
