using LocationDomain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocationRepository.CitryLogic
{
    public interface ICityLogic<T> where T : CityModel
    {
        IEnumerable<T> GetAllCity();
        Task<T> GetCity(int id);
        Task CreateCity(T entity);
        Task UpdateCity(T entity);
        Task DeleteCity(T entity);
        void RemoveCity(T entity);
        Task SaveChanges();
    }
}
