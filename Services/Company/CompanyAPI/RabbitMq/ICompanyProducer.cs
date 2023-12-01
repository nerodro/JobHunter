using CompanyAPI.ViewModel;

namespace CompanyAPI.RabbitMq
{
    public interface ICompanyProducer
    {
        public Task TakeAllVacanciesOfCompany<T>(List<VacancieViewModel> model);
        public Task CreateVacancieForCompany<T>(VacancieViewModel model);
        public Task DeleteVacancieForCompany<T>(VacancieViewModel model);
        public Task EditVacancieForCompany<T>(VacancieViewModel model);
        public Task<VacancieViewModel> TakeSingleVacancieForCompany(int id);
    }
}
