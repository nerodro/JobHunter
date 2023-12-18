using Grpc.Net.Client;
using CompanyAPI.ViewModel;

namespace CompanyAPI.ServiceGrpc
{
    public class LocationRpc
    {
        private readonly LocationServiceGrpc.LocationServiceGrpcClient _locationService;
        public LocationRpc()
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            var channel = GrpcChannel.ForAddress("https://locationapi:443",
                new GrpcChannelOptions { HttpHandler = handler });
            //var client = new LocationServiceGrpc.LocationServiceGrpcClient(channel);
            //var channel = GrpcChannel.ForAddress("https://localhost:7052");
            //var channel = GrpcChannel.ForAddress("https://locationapi:443");
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
