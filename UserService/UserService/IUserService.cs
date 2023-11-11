using UserDomain.Models;

namespace UserService.UserService
{
    public interface IUserService
    {
        IEnumerable<UserModel> GetAll();
        Task<UserModel> GetUser(int id);
        Task Create(UserModel model);
        Task Update(UserModel user);
        Task Delete(int id);
    }
}
