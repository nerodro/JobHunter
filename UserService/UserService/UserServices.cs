using UserDomain.Models;
using UserRepository.UserLogic;

namespace UserService.UserService
{
    public class UserServices : IUserService
    {
        private IUserLogic<UserModel> _user;
        public UserServices(IUserLogic<UserModel> user)
        {
            _user = user;
        }
        public IEnumerable<UserModel> GetAll()
        {
            return _user.GetAll();
        }
        public UserModel GetUser(int id)
        {
            return _user.Get(id);
        }
        public void Create(UserModel user)
        {
            _user.Create(user);
        }
        public void Update(UserModel user)
        {
            _user.Update(user);
        }
        public void Delete(int id)
        {
            UserModel user = GetUser(id);
            _user.Delete(user);
            _user.SaveChanges();
        }
    }
}
