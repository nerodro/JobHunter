using Grpc.Core;
using UserDomain.Models;
using UserService.UserService;
using static UserServiceGrpc;

namespace UserAPI.ServiceGrpc
{
    public class UserRpc : UserServiceGrpcBase
    {
        private readonly IUserService _UserService;
        public UserRpc(IUserService UserService)
        {
            _UserService = UserService;
        }
        public async override Task<UserResponseGrpc> GetUserById(UserRequestGrpc request, ServerCallContext context)
        {
            int UserId = request.UserId;
            UserModel model = await _UserService.GetUser(UserId);

            if (model == null)
            {
                throw new RpcException(new Status(StatusCode.Cancelled, "Запрашиваемое резюме не найдено"));
            }

            UserGrpc User = new UserGrpc
            {
                UserId = model.Id,
                UserName = model.Name
            };
            var response = new UserResponseGrpc
            {
                User = User
            };

            return await Task.FromResult(response);
        }
    }
}
