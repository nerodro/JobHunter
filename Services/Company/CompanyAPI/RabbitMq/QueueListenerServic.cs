using CompanyAPI.RabbitMq;

namespace CompanyAPI.RabbitMq
{
    public class QueueListenerService : IHostedService 
    {
        private readonly ICompanyProducer _company;

        public QueueListenerService(ICompanyProducer company)
        {
            _company = company;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {

            Task.Run(() =>
            {
               // _company.TakeSingleVacancieForCompany();
            }, cancellationToken);

            return Task.CompletedTask;
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {

            return Task.CompletedTask;
        }
    }
}
