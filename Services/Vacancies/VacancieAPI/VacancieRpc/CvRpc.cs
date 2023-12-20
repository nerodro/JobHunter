using Grpc.Net.Client;
using VacancieAPI.ViewModel;

namespace VacancieAPI.VacancieRpc
{
    public class CvRpc
    {
        private readonly CvServiceGrpc.CvServiceGrpcClient _rpc;
        public CvRpc()
        {
            var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .AddEnvironmentVariables()
                    .Build();

            var grpcConnection = configuration.GetSection("Grpc:CvHttp").Value;
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            var channel = GrpcChannel.ForAddress(grpcConnection,
                new GrpcChannelOptions { HttpHandler = handler });
            _rpc = new CvServiceGrpc.CvServiceGrpcClient(channel);
        }
        public async Task<CvViewModel> GetCv(int CvId)
        {
            var request = new CvRequestGrpc
            {
                CvId = CvId
            };

            var response = _rpc.GetCvById(request);
            CvViewModel Cv = new CvViewModel
            {
                Id = response.Cv.CvId,
                CvName = response.Cv.CvName
            };
            return Cv;
        }
    }
}
