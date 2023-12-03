using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading.Channels;
using VacancieAPI.ViewModel;
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
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public VacancieProducer(IVacancieService vacancyService, IConnection rabbitMqConnection, IModel rabbitMqChannel, IServiceScopeFactory serviceScopeFactory)
        {
            _VacancieService = vacancyService;
            _rabbitMqConnection = rabbitMqConnection;
            _rabbitMqChannel = rabbitMqChannel;
            _serviceScopeFactory = serviceScopeFactory;
        }
        public async Task SendSingleVacancie()
        {
            var connectionFactory = new ConnectionFactory() { HostName = "localhost" };

            using (var connection = connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                // Создаем очередь ответов
                channel.QueueDeclare("vacancy_requests_ask", false, false, false, null);
                channel.QueueDeclare("company_vacancies_response_queue", false, false, false, null);

                // Создаем обработчик для сообщений из очереди ответов
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += async (model, ea) =>
                {
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        // Resolve the scoped service VacancyContext
                        var vacancyContext = scope.ServiceProvider.GetRequiredService<VacancyContext>();
                        // Получаем сообщение из очереди и десериализуем его в ответ
                        var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                        var request = JsonConvert.DeserializeObject<VacancieViewModel>(message);
                        var response = await _VacancieService.GetVacancie((int)request.Id);
                        if (response != null)
                        {
                            var responseJson = JsonConvert.SerializeObject(response).ToArray();

                            var properties = _rabbitMqChannel.CreateBasicProperties();

                            _rabbitMqChannel.BasicPublish("", "company_vacancies_response_queue", properties, Encoding.UTF8.GetBytes(responseJson));
                        }
                    }
                };
                _rabbitMqChannel.BasicConsume("vacancy_requests_ask", true, consumer);
            }
        }

        public void Dispose()
        {
            // Удалите вызовы Dispose для _rabbitMqChannel и _rabbitMqConnection
        }
    }
}
