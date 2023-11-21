using LocationDomain.Model;

namespace LocationRepository.CountryLogic
{
    public interface ICountryLogic<T> where T : CountryModel
    {
        IEnumerable<T> GetAllCountry();
        Task<T> GetCountry(int id);
        Task CreateCountry(T entity);
        Task UpdateCountry(T entity);
        Task DeleteCountry(T entity);
        void RemoveCountry(T entity);
        Task SaveChanges();
    }
}