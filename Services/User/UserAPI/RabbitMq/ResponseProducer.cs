using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using UserAPI.ViewModel;
using Newtonsoft.Json;

namespace UserAPI.RabbitMq
{
    public class ResponseProducer : IResponseProducer
    {
        private IModel _rabbitMqChannel;
        public ResponseProducer(IModel rabbitMqChannel)
        {
            _rabbitMqChannel = rabbitMqChannel;
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

        public Task<string> CreateReposneForUser(ResponseViewModel model)
        {
            var request = model;
            var requestJson = JsonConvert.SerializeObject(request);
            var correlationId = Guid.NewGuid().ToString();
            var body = Encoding.UTF8.GetBytes(requestJson);
            var responseQueueName = _rabbitMqChannel.QueueDeclare().QueueName;

            var properties = _rabbitMqChannel.CreateBasicProperties();
            properties.ReplyTo = responseQueueName;
            properties.CorrelationId = correlationId;
            _rabbitMqChannel.BasicPublish("", "response_requests_create_response", properties, body);
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
            _rabbitMqChannel.BasicConsume("User_Responses_response_create_queue", true, consumer);
            if (!responseWaiter.Wait(TimeSpan.FromSeconds(10)))
            {
                throw new Exception("Timeout waiting for Responses response");

            }
            return Task.FromResult(responsetext);
        }

        public Task<string> DeleteResponseForUser(int id)
        {
            var request = new ResponseViewModel
            {
                Id = id,
            };
            var requestJson = JsonConvert.SerializeObject(request);
            var correlationId = Guid.NewGuid().ToString();
            var body = Encoding.UTF8.GetBytes(requestJson);
            var responseQueueName = _rabbitMqChannel.QueueDeclare().QueueName;

            var properties = _rabbitMqChannel.CreateBasicProperties();
            properties.ReplyTo = responseQueueName;
            properties.CorrelationId = correlationId;
            _rabbitMqChannel.BasicPublish("", "response_requests_delete_response", properties, body);
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
            _rabbitMqChannel.BasicConsume("User_Responses_response_delete_queue", true, consumer);
            if (!responseWaiter.Wait(TimeSpan.FromSeconds(10)))
            {
                throw new Exception("Timeout waiting for Responses response");

            }
            return Task.FromResult(responsetext);
        }

        public Task<string> EditResponseForUser(ResponseViewModel model)
        {
            var request = model;
            var requestJson = JsonConvert.SerializeObject(request);
            var correlationId = Guid.NewGuid().ToString();
            var body = Encoding.UTF8.GetBytes(requestJson);
            var responseQueueName = _rabbitMqChannel.QueueDeclare().QueueName;

            var properties = _rabbitMqChannel.CreateBasicProperties();
            properties.ReplyTo = responseQueueName;
            properties.CorrelationId = correlationId;
            _rabbitMqChannel.BasicPublish("", "response_requests_edit_response", properties, body);
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
            _rabbitMqChannel.BasicConsume("User_Responses_response_edit_queue", true, consumer);
            if (!responseWaiter.Wait(TimeSpan.FromSeconds(10)))
            {
                throw new Exception("Timeout waiting for Responses response");

            }
            return Task.FromResult(responsetext);
        }
        public IEnumerable<ResponseViewModel> TakeAllResponseOfUser(int CvId)
        {
            var request = new ResponseViewModel
            {
                CvId = CvId,
            };
            var requestJson = JsonConvert.SerializeObject(request);
            var correlationId = Guid.NewGuid().ToString();
            var body = Encoding.UTF8.GetBytes(requestJson);
            var responseQueueName = _rabbitMqChannel.QueueDeclare().QueueName;

            var properties = _rabbitMqChannel.CreateBasicProperties();
            properties.ReplyTo = responseQueueName;
            properties.CorrelationId = correlationId;
            _rabbitMqChannel.BasicPublish("", "response_requests_ask_User", properties, body);
            var responseWaiter = new ManualResetEventSlim(false);

            List<ResponseViewModel> modelvac = new List<ResponseViewModel>();
            var consumer = new EventingBasicConsumer(_rabbitMqChannel);
            consumer.Received += (model, ea) =>
            {
                var responseMessage = Encoding.UTF8.GetString(ea.Body.ToArray());
                var response = JsonConvert.DeserializeObject<List<ResponseViewModel>>(responseMessage)!;
                modelvac = response;
                responseWaiter.Set();
            };
            _rabbitMqChannel.BasicConsume("User_Responses_response_all_queue", true, consumer);
            if (!responseWaiter.Wait(TimeSpan.FromSeconds(10)))
            {
                throw new Exception("Timeout waiting for Responses response");

            }
            return modelvac;
        }

        public async Task<ResponseViewModel> TakeSingleResponseForUser(int id)
        {
            var request = new ResponseViewModel
            {
                Id = id,
            };
            var requestJson = JsonConvert.SerializeObject(request);
            var correlationId = Guid.NewGuid().ToString();
            var body = Encoding.UTF8.GetBytes(requestJson);
            var responseQueueName = _rabbitMqChannel.QueueDeclare().QueueName;

            var properties = _rabbitMqChannel.CreateBasicProperties();
            properties.ReplyTo = responseQueueName;
            properties.CorrelationId = correlationId;
            _rabbitMqChannel.BasicPublish("", "response_requests_get_response", properties, body);
            var responseWaiter = new ManualResetEventSlim(false);

            ResponseViewModel modelvac = default(ResponseViewModel)!;
            var consumer = new EventingBasicConsumer(_rabbitMqChannel);
            consumer.Received += (model, ea) =>
            {
                var responseMessage = Encoding.UTF8.GetString(ea.Body.ToArray());
                var response = JsonConvert.DeserializeObject<ResponseViewModel>(responseMessage);
                if (response != null)
                {
                    modelvac = response;
                }
                responseWaiter.Set(); 
            }; 
            _rabbitMqChannel.BasicConsume("User_Responses_response_queue", true, consumer);
            if (!responseWaiter.Wait(TimeSpan.FromSeconds(10)))
            {
                throw new Exception("Timeout waiting for Responses response");

            }
            return modelvac;
        }
    }
}
