﻿using CompanyAPI.ViewModel;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace CompanyAPI.RabbitMq
{
    public class CompanyProducer : ICompanyProducer
    {
        private IModel _rabbitMqChannel;
        //public CompanyProducer(IModel rabbitMqChannel)
        //{
            

        //    // Создание очереди для отправки сообщений
            
        //}

        public Task CreateVacancieForCompany<T>(VacancieViewModel model)
        {
            throw new NotImplementedException();
        }

        public Task DeleteVacancieForCompany<T>(VacancieViewModel model)
        {
            throw new NotImplementedException();
        }

        public Task EditVacancieForCompany<T>(VacancieViewModel model)
        {
            throw new NotImplementedException();
        }

        public Task TakeAllVacanciesOfCompany<T>(List<VacancieViewModel> model)
        {
            throw new NotImplementedException();
        }

        public async Task<VacancieViewModel> TakeSingleVacancieForCompany(int id)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            var connection = factory.CreateConnection();
            _rabbitMqChannel = connection.CreateModel();
            _rabbitMqChannel.QueueDeclare("vacancy_requests_ask", false, false, false, null);
            _rabbitMqChannel.QueueDeclare("company_vacancies_response_queue", false, false, false, null);
            var request = new VacancieViewModel
            {
                Id = id,
            };
            var requestJson = JsonConvert.SerializeObject(request);
            var message = $"CompanyId:{id}";
            var correlationId = Guid.NewGuid().ToString();
            var body = Encoding.UTF8.GetBytes(requestJson);
            var responseQueueName = _rabbitMqChannel.QueueDeclare().QueueName;

            var properties =  _rabbitMqChannel.CreateBasicProperties();
            properties.ReplyTo = responseQueueName;
            properties.CorrelationId = correlationId;
            _rabbitMqChannel.BasicPublish("", "vacancy_requests_ask", properties, body);
            var responseWaiter = new ManualResetEventSlim(false);

            VacancieViewModel modelvac = new VacancieViewModel();
            var consumer = new AsyncEventingBasicConsumer(_rabbitMqChannel);
            consumer.Received += async(model, ea) =>
            {
                var responseMessage = Encoding.UTF8.GetString(ea.Body.ToArray());
                var modelresp = JsonConvert.DeserializeObject<VacancieViewModel>(responseMessage);
                responseWaiter.Set();
            };
            _rabbitMqChannel.BasicConsume("company_vacancies_response_queue", true, consumer);
            if (!responseWaiter.Wait(TimeSpan.FromSeconds(10)))
            {

            }
            return modelvac;
        }
    }
}