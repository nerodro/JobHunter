using LocationDomain.Model;
using LocationRepository.CountryLogic;

namespace LocationService.CountryService
{
    public class CountryService : ICountryService
    {
        private readonly ICountryLogic<CountryModel> _CountryService;
        public CountryService(ICountryLogic<CountryModel> CountryService)
        {
            _CountryService = CountryService;
        }
        public IEnumerable<CountryModel> GetAllCountry()
        {
            return _CountryService.GetAllCountry();
        }
        public async Task<CountryModel> GetCountry(int id)
        {
            return await _CountryService.GetCountry(id);
        }
        public async Task CreateCountry(CountryModel model)
        {
            await _CountryService.CreateCountry(model);
        }
        public async Task UpdateCountry(CountryModel model)
        {
            await _CountryService.UpdateCountry(model);
        }
        public async Task DeleteCountry(int id)
        {
            CountryModel model = await _CountryService.GetCountry(id);
            await _CountryService.DeleteCountry(model);
            await _CountryService.SaveChanges();
        }
    }
}
