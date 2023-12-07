namespace ResponseAPI.RabbitMq
{
    public interface IResponseProducer
    {
        public Task SendSingleResponse();
        public Task CreateNewResponse();
        public Task DeleteResponse();
        public Task UpdateResponse();
        public Task SendResponseForUser();
    }
}
