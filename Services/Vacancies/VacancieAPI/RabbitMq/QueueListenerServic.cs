﻿using ResponseAPI.RabbitMq;

namespace VacancieAPI.RabbitMq
{
    public class QueueListenerService : IHostedService 
    {
        private readonly IVacancieProducercs _vacancie;

        public QueueListenerService(IVacancieProducercs vacancie)
        {
            _vacancie = vacancie;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {

            Task.Run(() =>
            {
                _vacancie.SendSingleVacancie();
                _vacancie.CreateNewVacancie();
                _vacancie.UpdateVacancie();
                _vacancie.DeleteVacancie();
                _vacancie.SendVacancieForCompany();
            }, cancellationToken);

            return Task.CompletedTask;
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {

            return Task.CompletedTask;
        }
    }
}
