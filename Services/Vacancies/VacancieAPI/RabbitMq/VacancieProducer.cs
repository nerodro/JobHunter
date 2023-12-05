using MassTransit;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading.Channels;
using VacancieAPI.ViewModel;
using VacancieDomain.Model;
using VacancieRepository;
using VacancieService.VacancieService;
using VacancieService.VacancyService;

namespace VacancieAPI.RabbitMq
{
    public class VacancieProducer : IVacancieProducercs, IDisposable
    {
        private readonly IVacancieService _VacancieService;
        private readonly IConnection _rabbitMqConnection;
        private IModel _rabbitMqChannel;
        public VacancieProducer(IVacancieService vacancyService, IConnection rabbitMqConnection, IModel rabbitMqChannel)
        {
            _VacancieService = vacancyService;
            _rabbitMqConnection = rabbitMqConnection;
            _rabbitMqChannel = rabbitMqChannel;
            var factory = new ConnectionFactory() { HostName = "localhost" };
            var connection = factory.CreateConnection();
            _rabbitMqChannel = connection.CreateModel();
            _rabbitMqChannel.QueueDeclare("vacancy_requests_ask", false, false, false, null);
            _rabbitMqChannel.QueueDeclare("company_vacancies_response_queue", false, false, false, null);
        }
        public async Task SendSingleVacancie()
        {
            var consumer = new EventingBasicConsumer(_rabbitMqChannel);
            consumer.Received += async (model, ea) =>
            {
                // Получаем сообщение из очереди и десериализуем его в ответ
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                var request = JsonConvert.DeserializeObject<VacancieViewModel>(message);
                VacancieModel res = new VacancieModel();
                var response = await _VacancieService.GetVacancie((int)request.Id);
                if (response != null)
                {
                    var responseJson = JsonConvert.SerializeObject(response).ToArray();

                    var properties = _rabbitMqChannel.CreateBasicProperties();

                    _rabbitMqChannel.BasicPublish("", "company_vacancies_response_queue", properties, Encoding.UTF8.GetBytes(responseJson));
                }
            };
            _rabbitMqChannel.BasicConsume("vacancy_requests_ask", true, consumer);
        }

        public void Dispose()
        {
        }
    }
}
