using LocationDomain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocationService.CountryService
{
    public interface ICountryService
    {
        IEnumerable<CountryModel> GetAllCountry();
        Task<CountryModel> GetCountry(int id);
        Task CreateCountry(CountryModel model);
        Task UpdateCountry(CountryModel model);
        Task DeleteCountry(int id);
    }
}
