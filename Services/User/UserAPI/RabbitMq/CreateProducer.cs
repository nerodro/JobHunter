using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using UserAPI.ViewModel;

namespace UserAPI.RabbitMq
{
    public class CreateProducer : ICreateProducer
    {
        private IModel _rabbitMqChannel;
        public CreateProducer(IModel rabbitMqChannel)
        {
            _rabbitMqChannel = rabbitMqChannel;
            _rabbitMqChannel.QueueDeclare("request_export_cv", false, false, false, null);
            _rabbitMqChannel.QueueDeclare("response_export_cv", false, false, false, null);
        }
        public Task GeneratePdfCv(CvViewModel model)
        {
            var request = model;
            var requestJson = JsonConvert.SerializeObject(request);
            var correlationId = Guid.NewGuid().ToString();
            var body = Encoding.UTF8.GetBytes(requestJson);
            var responseQueueName = _rabbitMqChannel.QueueDeclare().QueueName;

            var properties = _rabbitMqChannel.CreateBasicProperties();
            properties.ReplyTo = responseQueueName;
            properties.CorrelationId = correlationId;
            _rabbitMqChannel.BasicPublish("", "request_export_cv", properties, body);
            var responseWaiter = new ManualResetEventSlim(false);

            string responsetext = default!;
            var consumer = new EventingBasicConsumer(_rabbitMqChannel);
            consumer.Received += (model, ea) =>
            {
                var responseMessage = Encoding.UTF8.GetString(ea.Body.ToArray());
                var response = JsonConvert.DeserializeObject(responseMessage)!.ToString();
                if (response != null)
                {
                    responsetext = response;
                }
                responseWaiter.Set();
            };
            _rabbitMqChannel.BasicConsume("response_export_cv", true, consumer);
            if (!responseWaiter.Wait(TimeSpan.FromSeconds(10)))
            {
                throw new Exception("Timeout waiting for Responses response");

            }
            return Task.FromResult(responsetext);
        }
    }
}
