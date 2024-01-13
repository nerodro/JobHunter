using CompanyAPI.ViewModel;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace CompanyAPI.RabbitMq
{
    public class CompanyProducer : ICompanyProducer
    {
        private IModel _rabbitMqChannel;
        public CompanyProducer(IModel rabbitMqChannel)
        {
            _rabbitMqChannel = rabbitMqChannel;
            _rabbitMqChannel.QueueDeclare("vacancy_requests_get_vacancy", false, false, false, null);
            _rabbitMqChannel.QueueDeclare("vacancy_requests_edit_vacancy", false, false, false, null);
            _rabbitMqChannel.QueueDeclare("vacancy_requests_create_vacancy", false, false, false, null);
            _rabbitMqChannel.QueueDeclare("vacancy_requests_delete_vacancy", false, false, false, null);
            _rabbitMqChannel.QueueDeclare("vacancy_requests_ask_company", false, false, false, null);
            _rabbitMqChannel.QueueDeclare("vacancy_requests_ask", false, false, false, null);
            _rabbitMqChannel.QueueDeclare("company_vacancies_response_queue", false, false, false, null);
            _rabbitMqChannel.QueueDeclare("company_vacancies_response_create_queue", false, false, false, null);
            _rabbitMqChannel.QueueDeclare("company_vacancies_response_edit_queue", false, false, false, null);
            _rabbitMqChannel.QueueDeclare("company_vacancies_response_delete_queue", false, false, false, null);
            _rabbitMqChannel.QueueDeclare("company_vacancies_response_all_queue", false, false, false, null);
        }

        public Task<string> CreateVacancieForCompany(VacancieViewModel model)
        {
            var request = model;
            var requestJson = JsonConvert.SerializeObject(request);
            var correlationId = Guid.NewGuid().ToString();
            var body = Encoding.UTF8.GetBytes(requestJson);
            var responseQueueName = _rabbitMqChannel.QueueDeclare().QueueName;

            var properties = _rabbitMqChannel.CreateBasicProperties();
            properties.ReplyTo = responseQueueName;
            properties.CorrelationId = correlationId;
            _rabbitMqChannel.BasicPublish("", "vacancy_requests_create_vacancy", properties, body);
            var responseWaiter = new ManualResetEventSlim(false);

            //string responsetext = default!; 
            //var consumer = new EventingBasicConsumer(_rabbitMqChannel);
            //consumer.Received += (model, ea) =>
            //{
            //    var responseMessage = Encoding.UTF8.GetString(ea.Body.ToArray());
            //    var response = JsonConvert.DeserializeObject(responseMessage)!.ToString();
            //    if (response != null)
            //    {
            //        responsetext = response;
            //    }
            //    responseWaiter.Set();
            //};
            //_rabbitMqChannel.BasicConsume("company_vacancies_response_create_queue", true, consumer);
            //if (!responseWaiter.Wait(TimeSpan.FromSeconds(10)))
            //{
            //    throw new Exception("Timeout waiting for vacancies response");

            //}
            return Task.FromResult("Запрос на создание вакансий был отправлен");
        }

        public Task<string> DeleteVacancieForCompany(int id)
        {
            var request = new VacancieViewModel
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
            _rabbitMqChannel.BasicPublish("", "vacancy_requests_delete_vacancy", properties, body);
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
            _rabbitMqChannel.BasicConsume("company_vacancies_response_delete_queue", true, consumer);
            if (!responseWaiter.Wait(TimeSpan.FromSeconds(10)))
            {
                throw new Exception("Timeout waiting for vacancies response");

            }
            return Task.FromResult(responsetext);
        }

        public Task<string> EditVacancieForCompany(VacancieViewModel model)
        {
            var request = model;
            var requestJson = JsonConvert.SerializeObject(request);
            var correlationId = Guid.NewGuid().ToString();
            var body = Encoding.UTF8.GetBytes(requestJson);
            var responseQueueName = _rabbitMqChannel.QueueDeclare().QueueName;

            var properties = _rabbitMqChannel.CreateBasicProperties();
            properties.ReplyTo = responseQueueName;
            properties.CorrelationId = correlationId;
            _rabbitMqChannel.BasicPublish("", "vacancy_requests_edit_vacancy", properties, body);
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
            _rabbitMqChannel.BasicConsume("company_vacancies_response_edit_queue", true, consumer);
            if (!responseWaiter.Wait(TimeSpan.FromSeconds(10)))
            {
                throw new Exception("Timeout waiting for vacancies response");

            }
            return Task.FromResult(responsetext)!;
        }

        public IEnumerable<VacancieViewModel> TakeAllVacanciesOfCompany(int companyId)
        {
            var request = new VacancieViewModel
            {
                CompanyId = companyId,
            };
            var requestJson = JsonConvert.SerializeObject(request);
            var correlationId = Guid.NewGuid().ToString();
            var body = Encoding.UTF8.GetBytes(requestJson);
            var responseQueueName = _rabbitMqChannel.QueueDeclare().QueueName;

            var properties = _rabbitMqChannel.CreateBasicProperties();
            properties.ReplyTo = responseQueueName;
            properties.CorrelationId = correlationId;
            _rabbitMqChannel.BasicPublish("", "vacancy_requests_ask_company", properties, body);
            var responseWaiter = new ManualResetEventSlim(false);

            List<VacancieViewModel> modelvac = new List<VacancieViewModel>();
            var consumer = new EventingBasicConsumer(_rabbitMqChannel);
            consumer.Received += (model, ea) =>
            {
                var responseMessage = Encoding.UTF8.GetString(ea.Body.ToArray());
                var response = JsonConvert.DeserializeObject<List<VacancieViewModel>>(responseMessage);
                modelvac = response!;
                responseWaiter.Set();
            };
            _rabbitMqChannel.BasicConsume("company_vacancies_response_all_queue", true, consumer);
            if (!responseWaiter.Wait(TimeSpan.FromSeconds(10)))
            {
                throw new Exception("Timeout waiting for vacancies response");

            }
            return modelvac;
        }

        public async Task<VacancieViewModel> TakeSingleVacancieForCompany(int id)
        {
            var request = new VacancieViewModel
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
            _rabbitMqChannel.BasicPublish("", "vacancy_requests_ask", properties, body);
            var responseWaiter = new ManualResetEventSlim(false);

            VacancieViewModel modelvac = default(VacancieViewModel)!;
            var consumer = new EventingBasicConsumer(_rabbitMqChannel);
            consumer.Received += (model, ea) =>
            {
                var responseMessage = Encoding.UTF8.GetString(ea.Body.ToArray());
                var response = JsonConvert.DeserializeObject<VacancieViewModel>(responseMessage);
                if (response != null)
                {
                    modelvac = response;
                }
                responseWaiter.Set();
            };
            _rabbitMqChannel.BasicConsume("company_vacancies_response_queue", true, consumer);
            if (!responseWaiter.Wait(TimeSpan.FromSeconds(10)))
            {
                throw new Exception("Timeout waiting for vacancies response");

            }
            return modelvac;
        }
    }
}
