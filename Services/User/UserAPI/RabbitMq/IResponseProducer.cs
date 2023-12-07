using UserAPI.ViewModel;

namespace UserAPI.RabbitMq
{
    public interface IResponseProducer
    {
        public IEnumerable<ResponseViewModel> TakeAllResponseOfUser(int UserId);
        public Task<string> CreateReposneForUser(ResponseViewModel model);
        public Task<string> DeleteResponseForUser(int id);
        public Task<string> EditResponseForUser(ResponseViewModel model);
        public Task<ResponseViewModel> TakeSingleResponseForUser(int id);
    }
}
