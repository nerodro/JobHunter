using RabbitMQ.Client;

namespace ExportAPI.RabbitMq
{
    public class ExportCv : IExportCv
    {
        private readonly IExportCv _exportCv;
        private IModel _rabbitMqChannel;
        public ExportCv(IExportCv exportCv, IModel rabbitMqChannel)
        {
            _exportCv = exportCv;
            _rabbitMqChannel = rabbitMqChannel;
            _rabbitMqChannel.QueueDeclare("request_export_cv", false, false, false, null);
            _rabbitMqChannel.QueueDeclare("response_export_cv", false, false, false, null);
        }
        public Task GenerateNewPDF()
        {
            throw new NotImplementedException();
        }
    }
}
