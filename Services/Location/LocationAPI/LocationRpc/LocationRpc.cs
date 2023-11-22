using Grpc.Core;
using LocationService.CityService;
using LocationService.CountryService;

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
                throw new RpcException(new Status(StatusCode.Cancelled, "Запрашиваемая категория не найдена"));
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
    }
}
