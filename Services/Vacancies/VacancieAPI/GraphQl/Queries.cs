﻿using HotChocolate.Data;
using VacancieAPI.ServiceGrpc;
using VacancieAPI.VacancieRpc;
using VacancieAPI.ViewModel;
using VacancieDomain.Model;
using VacancieService.VacancieService;

namespace VacancieAPI.GraphQl
{
    public class Queries
    {
        //private readonly LocationRpc _rpc;
        private readonly CompanyRpc _companyRpc;
        public Queries(LocationRpc rpc, CompanyRpc companyRpc)
        {
           // _rpc = rpc;
            _companyRpc = companyRpc;
        }
        [UseProjection]
        public IQueryable<VacancieViewModel> Read([Service] IVacancieService vacancieService)
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
                    // CompanyName = await GetCompanyName(u.CompanyId),
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
            var rpc = new LocationRpc(); // Создание экземпляра LocationRpc
            var country = rpc.GetCountryByIdSync(id);
            return country.CountryName;
        }
        private static string GetCityName(int id)
        {
            var rpc = new LocationRpc();
            var city =  rpc.GetCityByIdSync(id);
            return city.CityName;
        }
        private async Task<string> GetCompanyName(int id)
        {
            var company = await _companyRpc.GetCompany(id);
            return company.CompanyName;
        }
    }
}