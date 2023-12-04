using VacancieAPI.ViewModel;

namespace VacancieAPI.RabbitMq
{
    public interface IVacancieProducercs
    {
        public Task SendSingleVacancie();
    }
}
