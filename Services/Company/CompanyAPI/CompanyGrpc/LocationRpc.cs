using Grpc.Net.Client;
using CompanyAPI.ViewModel;

namespace CompanyAPI.ServiceGrpc
{
    public class LocationRpc
    {
        private readonly LocationServiceGrpc.LocationServiceGrpcClient _locationService;
        public LocationRpc()
        {
            var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .AddEnvironmentVariables()
                    .Build();

            var grpcConnection = configuration.GetSection("Grpc:LocationHttp").Value!;
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            var channel = GrpcChannel.ForAddress(grpcConnection,
                new GrpcChannelOptions { HttpHandler = handler });
            _locationService = new LocationServiceGrpc.LocationServiceGrpcClient(channel);
        }
        public async Task<CityViewModel> GetCityById(int CityId)
        {
            var request = new CityRequestGrpc
            {
                CityId = CityId
            };

            var response = _locationService.GetCityById(request);
            CityViewModel model = new CityViewModel
            {
                Id = response.City.CityId,
                CityName = response.City.CityName,
                CountryId = response.City.CountryId,
            };
            return model;
        }
        public async Task<CountryViewModel> GetCountryById(int CountryId)
        {
            var request = new CountryRequestGrpc
            {
                CountryId = CountryId
            };

            var response = _locationService.GetCountryById(request);
            CountryViewModel model = new CountryViewModel
            {
                Id = response.Country.CountryId,
                CountryName = response.Country.CountryName,
            };
            return model;
        }
    }
}
