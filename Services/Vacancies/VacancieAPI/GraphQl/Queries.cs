using HotChocolate.Data;
using VacancieAPI.ServiceGrpc;
using VacancieAPI.VacancieRpc;
using VacancieAPI.ViewModel;
using VacancieDomain.Model;
using VacancieService.VacancieService;

namespace VacancieAPI.GraphQl
{
    public class Queries
    {
        [UseProjection]
        public IQueryable<VacancieViewModel> ReadAllVacancies([Service] IVacancieService vacancieService)
        {
            if (vacancieService != null)
            {
                var vacancies = vacancieService.GetAllQuerie();

                var projectedVacancies = vacancies.Select(u => new VacancieViewModel
                {
                    Id = u.Id,
                    CityId = u.CityId,
                    CompanyId = u.CompanyId,
                    CountryId = u.CountryId,
                    AboutWork = u.AboutWork,
                    WorkName = u.WorkName,
                    Salary = u.Salary,
                    CountryName = GetCountryName(u.CountryId),
                    CityName =  GetCityName(u.CityId),
                    CompanyName = GetCompanyName(u.CompanyId),
                    Pinned = u.Pinned,
                });

                return projectedVacancies;
            }
            else
            {
                return Enumerable.Empty<VacancieViewModel>().AsQueryable();
            }
        }
        private static string GetCountryName(int id)
        {
            var rpc = new LocationRpc();
            var country = rpc.GetCountryById(id);
            return country.CountryName;
        }
        private static string GetCityName(int id)
        {
            var rpc = new LocationRpc();
            var city =  rpc.GetCityById(id);
            return city.CityName;
        }
        private static string GetCompanyName(int id)
        {
            var rpc = new CompanyRpc();
            var company = rpc.GetCompany(id);
            return company.CompanyName;
        }
    }
}
