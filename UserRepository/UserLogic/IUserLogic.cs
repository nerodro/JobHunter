using UserDomain.Models;

namespace UserRepository.UserLogic
{
    public interface IUserLogic<T> where T : UserModel
    {
        IAsyncEnumerable<T> GetAll();
        Task<T> Get(long id);
        Task Create(T entity);
        Task Update(T entity);
        Task Delete(T entity);
        void Remove(T entity);
        Task SaveChanges();
    }
}
