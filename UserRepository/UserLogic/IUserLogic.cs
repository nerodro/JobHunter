using UserDomain.Models;

namespace UserRepository.UserLogic
{
    public interface IUserLogic<T> where T : UserModel
    {
        IEnumerable<T> GetAll();
        T Get(long id);
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Remove(T entity);
        void SaveChanges();
    }
}
