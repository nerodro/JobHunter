using ExportAPI.RabbitMq;

namespace ExportAPI.RabbitMq
{
    public class QueueListenerService : IHostedService 
    {
        private readonly IExportCv _export;

        public QueueListenerService(IExportCv export)
        {
            _export = export;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {

            Task.Run(() =>
            {
                _export.GenerateNewPDF();
            }, cancellationToken);

            return Task.CompletedTask;
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {

            return Task.CompletedTask;
        }
    }
}
