using UserDomain.Models;

namespace UserRepository.CvLogic
{
    public interface ICvLogic<T> where T : CvModel
    {
        IEnumerable<T> GetAll();
        T Get(long id);
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
        void DeleteOfUser(List<T> entity);
        void Remove(T entity);
        void SaveChanges();
    }
}
