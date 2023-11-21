using LocationDomain.Model;
using LocationRepository.CitryLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocationService.CityService
{
    internal class CityService : ICityService
    {
        private readonly ICityLogic<CityModel> _cityService;
        public CityService(ICityLogic<CityModel> cityService)
        {
            _cityService = cityService;
        }
        public IEnumerable<CityModel> GetAllCity()
        {
            return _cityService.GetAllCity();
        }
        public async Task<CityModel> GetCity(int id)
        {
            return await _cityService.GetCity(id);
        }
        public async Task CreateCity(CityModel model)
        {
            await _cityService.CreateCity(model);
        }
        public async Task UpdateCity(CityModel model)
        {
            await _cityService.UpdateCity(model);
        }
        public async Task DeleteCity(int id)
        {
            CityModel model = await _cityService.GetCity(id);
            await _cityService.DeleteCity(model);
            await _cityService.SaveChanges();
        }
    }
}
