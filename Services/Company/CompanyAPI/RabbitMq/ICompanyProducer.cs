using CompanyAPI.ViewModel;

namespace CompanyAPI.RabbitMq
{
    public interface ICompanyProducer
    {
        public IEnumerable<VacancieViewModel> TakeAllVacanciesOfCompany(int companyId);
        public Task<string> CreateVacancieForCompany(VacancieViewModel model);
        public Task<string> DeleteVacancieForCompany(int id);
        public Task<string> EditVacancieForCompany(VacancieViewModel model);
        public Task<VacancieViewModel> TakeSingleVacancieForCompany(int id);
    }
}
