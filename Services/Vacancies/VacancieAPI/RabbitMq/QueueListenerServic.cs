using ResponseAPI.RabbitMq;

namespace VacancieAPI.RabbitMq
{
    public class QueueListenerService : IHostedService 
    {
        private readonly IVacancieProducercs _vacancie;
        private readonly IResponseProducer _response;

        public QueueListenerService(IVacancieProducercs vacancie, IResponseProducer response)
        {
            _vacancie = vacancie;
            _response = response;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {

            Task.Run(() =>
            {
                _vacancie.SendSingleVacancie();
                _vacancie.CreateNewVacancie();
                _vacancie.UpdateVacancie();
                _vacancie.DeleteVacancie();
                _vacancie.SendVacancieForCompany();
                _response.SendSingleResponse();
                _response.CreateNewResponse();
                _response.UpdateResponse();
                _response.DeleteResponse();
                _response.SendResponseForUser();
            }, cancellationToken);

            return Task.CompletedTask;
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {

            return Task.CompletedTask;
        }
    }
}
