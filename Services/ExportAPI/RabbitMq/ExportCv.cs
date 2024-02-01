using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using UserAPI.ViewModel;

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
        public async Task GenerateNewPDF()
        {
            var consumer = new EventingBasicConsumer(_rabbitMqChannel);
            consumer.Received += async (model, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                var request = JsonConvert.DeserializeObject<CvViewModel>(message)!;

            };
            _rabbitMqChannel.BasicConsume("request_export_cv", true, consumer);
        }

    }
}
