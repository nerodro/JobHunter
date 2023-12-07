using RabbitMQ.Client;
using ResponseAPI.RabbitMq;
using VacancieService.VacancieService;

namespace VacancieAPI.RabbitMq
{
    public class ResponseProducer : IResponseProducer
    {
        private readonly IVacancieService _VacancieService;
        private readonly IConnection _rabbitMqConnection;
        private IModel _rabbitMqChannel;
        public ResponseProducer(IVacancieService vacancyService, IConnection rabbitMqConnection, IModel rabbitMqChannel)
        {
            _VacancieService = vacancyService;
            _rabbitMqConnection = rabbitMqConnection;
            _rabbitMqChannel = rabbitMqChannel;
            var factory = new ConnectionFactory() { HostName = "localhost" };
            var connection = factory.CreateConnection();
            _rabbitMqChannel = connection.CreateModel();
            _rabbitMqChannel.QueueDeclare("response_requests_get_response", false, false, false, null);
            _rabbitMqChannel.QueueDeclare("response_requests_edit_response", false, false, false, null);
            _rabbitMqChannel.QueueDeclare("response_requests_create_response", false, false, false, null);
            _rabbitMqChannel.QueueDeclare("response_requests_delete_response", false, false, false, null);
            _rabbitMqChannel.QueueDeclare("response_requests_ask_User", false, false, false, null);
            _rabbitMqChannel.QueueDeclare("response_requests_ask", false, false, false, null);
            _rabbitMqChannel.QueueDeclare("User_Responses_response_queue", false, false, false, null);
            _rabbitMqChannel.QueueDeclare("User_Responses_response_create_queue", false, false, false, null);
            _rabbitMqChannel.QueueDeclare("User_Responses_response_edit_queue", false, false, false, null);
            _rabbitMqChannel.QueueDeclare("User_Responses_response_delete_queue", false, false, false, null);
            _rabbitMqChannel.QueueDeclare("User_Responses_response_all_queue", false, false, false, null);
        }

        public Task CreateNewResponse()
        {
            throw new NotImplementedException();
        }

        public Task DeleteResponse()
        {
            throw new NotImplementedException();
        }

        public Task SendResponseForUser()
        {
            throw new NotImplementedException();
        }

        public Task SendSingleResponse()
        {
            throw new NotImplementedException();
        }

        public Task UpdateResponse()
        {
            throw new NotImplementedException();
        }
    }
}
