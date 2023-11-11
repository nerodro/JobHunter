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
        public async Task<UserModel> GetUser(int id)
        {
            return await _user.Get(id);
        }
        public async Task Create(UserModel user)
        {
            await _user.Create(user);
        }
        public async Task Update(UserModel user)
        {
            await _user.Update(user);
        }
        public async Task Delete(int id)
        {
            UserModel user = await GetUser(id);
            await _user.Delete(user);
            await _user.SaveChanges();
        }
    }
}
