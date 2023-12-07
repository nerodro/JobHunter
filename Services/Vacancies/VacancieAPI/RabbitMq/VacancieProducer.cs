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
            _rabbitMqChannel.QueueDeclare("vacancy_requests_get_vacancy", false, false, false, null);
            _rabbitMqChannel.QueueDeclare("vacancy_requests_edit_vacancy", false, false, false, null);
            _rabbitMqChannel.QueueDeclare("vacancy_requests_create_vacancy", false, false, false, null);
            _rabbitMqChannel.QueueDeclare("vacancy_requests_delete_vacancy", false, false, false, null);
            _rabbitMqChannel.QueueDeclare("vacancy_requests_ask_company", false, false, false, null);
            _rabbitMqChannel.QueueDeclare("company_vacancies_response_queue", false, false, false, null);
            _rabbitMqChannel.QueueDeclare("vacancy_requests_ask", false, false, false, null);
            _rabbitMqChannel.QueueDeclare("company_vacancies_response_create_queue", false, false, false, null);
            _rabbitMqChannel.QueueDeclare("company_vacancies_response_edit_queue", false, false, false, null);
            _rabbitMqChannel.QueueDeclare("company_vacancies_response_delete_queue", false, false, false, null);
            _rabbitMqChannel.QueueDeclare("company_vacancies_response_all_queue", false, false, false, null);
        }
        public async Task SendSingleVacancie()
        {
            var consumer =  new EventingBasicConsumer(_rabbitMqChannel);
            consumer.Received += async (model, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                var request = JsonConvert.DeserializeObject<VacancieViewModel>(message);
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
        public async Task CreateNewVacancie()
        {
            var consumer = new EventingBasicConsumer(_rabbitMqChannel);
            consumer.Received += async (model, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                var request = JsonConvert.DeserializeObject<VacancieModel>(message);
                if (request != null)
                {
                    await _VacancieService.CreateVacancie(request);
                    var responseJson = JsonConvert.SerializeObject("Ok").ToString();

                    var properties = _rabbitMqChannel.CreateBasicProperties();

                    _rabbitMqChannel.BasicPublish("", "company_vacancies_response_create_queue", properties, Encoding.UTF8.GetBytes(responseJson));
                }
            };
            _rabbitMqChannel.BasicConsume("vacancy_requests_create_vacancy", true, consumer);
        }

        public void Dispose()
        {
        }

        public async Task DeleteVacancie()
        {
            var consumer = new EventingBasicConsumer(_rabbitMqChannel);
            consumer.Received += async (model, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                var request = JsonConvert.DeserializeObject<VacancieViewModel>(message);
                VacancieModel res = new VacancieModel();
                await _VacancieService.DeleteVacancie((int)request.Id);
                var responseJson = JsonConvert.SerializeObject("Ok").ToArray();

                var properties = _rabbitMqChannel.CreateBasicProperties();

                _rabbitMqChannel.BasicPublish("", "company_vacancies_response_delete_queue", properties, Encoding.UTF8.GetBytes(responseJson));
                
            };
            _rabbitMqChannel.BasicConsume("vacancy_requests_delete_vacancy", true, consumer);
        }

        public async Task UpdateVacancie()
        {
            var consumer = new EventingBasicConsumer(_rabbitMqChannel);
            consumer.Received += async (model, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                var request = JsonConvert.DeserializeObject<VacancieModel>(message);
                var mod = await _VacancieService.GetVacancie(request.Id);
                if (mod != null)
                {
                    mod = request;
                    await _VacancieService.UpdateVacancie(mod);
                    var responseJson = JsonConvert.SerializeObject("Ok").ToString();

                    var properties = _rabbitMqChannel.CreateBasicProperties();

                    _rabbitMqChannel.BasicPublish("", "company_vacancies_response_edit_queue", properties, Encoding.UTF8.GetBytes(responseJson));
                }
                
                
            };
            _rabbitMqChannel.BasicConsume("vacancy_requests_edit_vacancy", true, consumer);
        }

        public async Task SendVacancieForCompany()
        {
            var consumer = new EventingBasicConsumer(_rabbitMqChannel);
            consumer.Received += async (model, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                var request = JsonConvert.DeserializeObject<VacancieViewModel>(message);
                var response =  _VacancieService.GetForCompany((int)request.CompanyId);
                if (response != null)
                {
                    var responseJson = JsonConvert.SerializeObject(response).ToArray();

                    var properties = _rabbitMqChannel.CreateBasicProperties();

                    _rabbitMqChannel.BasicPublish("", "company_vacancies_response_all_queue", properties, Encoding.UTF8.GetBytes(responseJson));
                }
            };
            _rabbitMqChannel.BasicConsume("vacancy_requests_ask_company", true, consumer);
        }
    }
}
