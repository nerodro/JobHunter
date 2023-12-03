namespace VacancieAPI.RabbitMq
{
    public class QueueListenerService : IHostedService 
    {
        private readonly ILogger<QueueListenerService> _logger;
        private readonly IVacancieProducercs _vacancie;

        public QueueListenerService(/*ILogger<QueueListenerService> logger,*/ IVacancieProducercs vacancie)
        {
            //_logger = logger;
            _vacancie = vacancie;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            //_logger.LogInformation("Starting queue listener...");

            Task.Run(() =>
            {
                // Ваш код метода StartListening
                _vacancie.SendSingleVacancie();
            }, cancellationToken);

            return Task.CompletedTask;
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            //_logger.LogInformation("Stopping queue listener...");

            // Здесь можно добавить код для остановки обработки очереди, если это необходимо

            return Task.CompletedTask;
        }
    }
}
