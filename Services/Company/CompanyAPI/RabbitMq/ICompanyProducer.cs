using CompanyAPI.ViewModel;

namespace CompanyAPI.RabbitMq
{
    public interface ICompanyProducer
    {
        public Task<IEnumerable<VacancieViewModel>> TakeAllVacanciesOfCompany(List<VacancieViewModel> model);
        public Task CreateVacancieForCompany<T>(VacancieViewModel model);
        public Task DeleteVacancieForCompany(int id);
        public Task EditVacancieForCompany(VacancieViewModel model);
        public Task<VacancieViewModel> TakeSingleVacancieForCompany(int id);
    }
}
