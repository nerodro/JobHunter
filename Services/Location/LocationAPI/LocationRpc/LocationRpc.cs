using Grpc.Core;
using LocationDomain.Model;
using LocationService.CityService;
using LocationService.CountryService;
using static LocationServiceGrpc;

namespace LocationAPI.LocationRpc
{
    public class LocationRpc : LocationServiceGrpcBase
    {
        private readonly ICityService _City;
        private readonly ICountryService _Country;
        public LocationRpc(ICityService city, ICountryService country)
        {
            _City = city;
            _Country = country;
        }
        public async override Task<CityResponseGrpc> GetCityById(CityRequestGrpc request, ServerCallContext context)
        {
            int CityId = request.CityId;
            CityModel model = await _City.GetCity(CityId);

            if (model == null)
            {
                throw new RpcException(new Status(StatusCode.Cancelled, "Запрашиваемый город не найден"));
            }

            CityGrpc City = new CityGrpc
            {
                CityId = model.Id,
                CityName = model.CityName
            };
            var response = new CityResponseGrpc
            {
                City = City
            };
            return await Task.FromResult(response);
        }
        public async override Task<CountryResponseGrpc> GetCountryById(CountryRequestGrpc request, ServerCallContext context)
        {
            int CountryId = request.CountryId;
            CountryModel model = await _Country.GetCountry(CountryId);

            if (model == null)
            {
                throw new RpcException(new Status(StatusCode.Cancelled, "Запрашиваемая страна не найдена"));
            }

            CountryGrpc Country = new CountryGrpc
            {
                CountryId = model.Id,
                CountryName = model.CountryName
            };
            var response = new CountryResponseGrpc
            {
                Country = Country
            };

            return await Task.FromResult(response);
        }
    }
}
