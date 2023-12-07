using ResponseAPI.RabbitMq;

namespace VacancieAPI.RabbitMq
{
    public class QueueListenerResponse : IHostedService
    {
        private readonly IResponseProducer _response;

        public QueueListenerResponse(IResponseProducer response)
        {
            _response = response;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {

            Task.Run(() =>
            {
                _response.SendResponseForUser();
                _response.CreateNewResponse();
                _response.UpdateResponse();
                _response.DeleteResponse();
                _response.SendSingleResponse();
            }, cancellationToken);

            return Task.CompletedTask;
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {

            return Task.CompletedTask;
        }
    }
}
