using ExportAPI.ExportPdf;
using ExportAPI.ViewModel;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace ExportAPI.RabbitMq
{
    public class ExportCv : IExportCv
    {
        private IModel _rabbitMqChannel;
        private readonly ExportClass _exportClass;
        public ExportCv(IModel rabbitMqChannel, ExportClass exportClass)
        {
            _rabbitMqChannel = rabbitMqChannel;
            _exportClass = exportClass;
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
                var response = _exportClass.GeneratePdfCv(request);
                var responseJson = JsonConvert.SerializeObject(response).ToArray();

                var properties = _rabbitMqChannel.CreateBasicProperties();
                _rabbitMqChannel.BasicPublish("", "response_export_cv", properties, Encoding.UTF8.GetBytes(responseJson));

            };
            _rabbitMqChannel.BasicConsume("request_export_cv", true, consumer);
        }

    }
}
