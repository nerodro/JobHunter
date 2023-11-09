using UserDomain.Models;

namespace UserService.UserService
{
    public interface IUserService
    {
        IEnumerable<UserModel> GetAll();
        UserModel GetUser(int id);
        void Create(UserModel user);
        void Update(UserModel user);
        void Delete(int id);
    }
}
