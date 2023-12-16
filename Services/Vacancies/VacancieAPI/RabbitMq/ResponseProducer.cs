using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ResponseAPI.RabbitMq;
using System.Text;
using VacancieAPI.ViewModel;
using VacancieDomain.Model;
using VacancieService.ResponseService;
using VacancieService.VacancieService;

namespace ResponseAPI.RabbitMq
{
    public class ResponseProducer : IResponseProducer
    {
        private readonly IResponseService _ResponseService;
        private IModel _rabbitMqChannel;
        public ResponseProducer(IResponseService vacancyService, IModel rabbitMqChannel)
        {
            _ResponseService = vacancyService;
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
        public async Task SendSingleResponse()
        {
            var consumer = new EventingBasicConsumer(_rabbitMqChannel);
            consumer.Received += async (model, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                var request = JsonConvert.DeserializeObject<ResponseViewModel>(message);
                var response = await _ResponseService.GetResponse((int)request.Id);
                if (response != null)
                {
                    var responseJson = JsonConvert.SerializeObject(response).ToArray();

                    var properties = _rabbitMqChannel.CreateBasicProperties();

                    _rabbitMqChannel.BasicPublish("", "User_Responses_response_queue", properties, Encoding.UTF8.GetBytes(responseJson));
                }
            };
            _rabbitMqChannel.BasicConsume("response_requests_get_response", true, consumer);
        }
        public async Task CreateNewResponse()
        {
            var consumer = new EventingBasicConsumer(_rabbitMqChannel);
            consumer.Received += async (model, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                var request = JsonConvert.DeserializeObject<ResponseModel>(message);
                if (request != null)
                {
                    await _ResponseService.CreateResponse(request);
                    var responseJson = JsonConvert.SerializeObject("Ok").ToString();

                    var properties = _rabbitMqChannel.CreateBasicProperties();

                    _rabbitMqChannel.BasicPublish("", "User_Responses_response_create_queue", properties, Encoding.UTF8.GetBytes(responseJson));
                }
            };
            _rabbitMqChannel.BasicConsume("response_requests_create_response", true, consumer);
        }


        public async Task DeleteResponse()
        {
            var consumer = new EventingBasicConsumer(_rabbitMqChannel);
            consumer.Received += async (model, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                var request = JsonConvert.DeserializeObject<ResponseViewModel>(message);
                ResponseModel res = new ResponseModel();
                await _ResponseService.DeleteResponse((int)request.Id);
                var responseJson = JsonConvert.SerializeObject("Ok").ToArray();

                var properties = _rabbitMqChannel.CreateBasicProperties();

                _rabbitMqChannel.BasicPublish("", "User_Responses_response_delete_queue", properties, Encoding.UTF8.GetBytes(responseJson));

            };
            _rabbitMqChannel.BasicConsume("response_requests_delete_response", true, consumer);
        }

        public async Task UpdateResponse()
        {
            var consumer = new EventingBasicConsumer(_rabbitMqChannel);
            consumer.Received += async (model, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                var request = JsonConvert.DeserializeObject<ResponseModel>(message);
                var mod = await _ResponseService.GetResponse(request.Id);
                if (mod != null)
                {
                    mod = request;
                    await _ResponseService.UpdateResponse(mod);
                    var responseJson = JsonConvert.SerializeObject("Ok").ToString();

                    var properties = _rabbitMqChannel.CreateBasicProperties();

                    _rabbitMqChannel.BasicPublish("", "User_Responses_response_edit_queue", properties, Encoding.UTF8.GetBytes(responseJson));
                }


            };
            _rabbitMqChannel.BasicConsume("response_requests_edit_response", true, consumer);
        }

        public async Task SendResponseForUser()
        {
            var consumer = new EventingBasicConsumer(_rabbitMqChannel);
            consumer.Received += async (model, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                var request = JsonConvert.DeserializeObject<ResponseViewModel>(message);
                var response = _ResponseService.GetAll(/*(int)request.VacancieId*/);
                if (response != null)
                {
                    var responseJson = JsonConvert.SerializeObject(response).ToArray();

                    var properties = _rabbitMqChannel.CreateBasicProperties();

                    _rabbitMqChannel.BasicPublish("", "User_Responses_response_all_queue", properties, Encoding.UTF8.GetBytes(responseJson));
                }
            };
            _rabbitMqChannel.BasicConsume("response_requests_ask_User", true, consumer);
        }
    }
}
