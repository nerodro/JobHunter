using UserDomain.Models;

namespace UserRepository.CvLogic
{
    public interface ICvLogic<T> where T : CvModel
    {
        IAsyncEnumerable<T> GetAll();
        Task<T> Get(long id);
        Task Create(T entity);
        Task Update(T entity);
        Task Delete(T entity);
        void Remove(T entity);
        Task SaveChanges();
        void DeleteOfUser(List<T> entity);
    }
}
