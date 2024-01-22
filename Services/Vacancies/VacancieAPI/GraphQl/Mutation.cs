using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using VacancieAPI.ServiceGrpc;
using VacancieAPI.VacancieRpc;
using VacancieAPI.ViewModel;
using VacancieDomain.Model;
using VacancieService.VacancieService;

namespace VacancieAPI.GraphQl
{
    public class Mutation
    {
        public async Task<VacancieViewModel> CreateVacancie([Service]VacancieViewModel model, IVacancieService vacancieService)
        {
            VacancieModel vacancie = new VacancieModel
            {
                CityId = GetCityId(model.CityId),
                CountryId = GetCountryId(model.CountryId),
                AboutWork = model.AboutWork.Trim(),
                WorkName = model.WorkName.Trim(),
                CompanyId = GetCompanyId(model.CompanyId),
                Salary = model.Salary,
                Pinned = 0,
            };
            if (model.CompanyId != 0 && model.WorkName != null)
            {
                await vacancieService.CreateVacancie(vacancie);
                return model;
            }
            throw new ArgumentException("Не все обязательные поля были заполнены");
        }
        private static int GetCityId(int id)
        {
            var rpc = new LocationRpc();
            var city = rpc.GetCityById(id);
            return (int)city.Id;
        }
        private int GetCountryId(int id)
        {
            var rpc = new LocationRpc();
            var country = rpc.GetCountryById(id);
            return (int)country.Id;
        }
        private static int GetCompanyId(int id)
        {
            var rpc = new CompanyRpc();
            var company = rpc.GetCompany(id);
            return (int)company.Id;
        }
    }
}
