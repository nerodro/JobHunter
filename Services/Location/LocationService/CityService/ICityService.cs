using LocationDomain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocationService.CityService
{
    public interface ICityService
    {
        IEnumerable<CityModel> GetAllCity();
        Task<CityModel> GetCity(int id);
        Task CreateCity(CityModel model);
        Task UpdateCity(CityModel model);
        Task DeleteCity(int id);
    }
}
