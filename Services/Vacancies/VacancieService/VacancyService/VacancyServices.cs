using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacancieDomain.Model;
using VacancieRepository.VacancieLogic;
using VacancieService.VacancieService;

namespace VacancieService.VacancyService
{
    public class VacancyServices : IVacancieService
    {
        private readonly IVacancieLogic<VacancieModel> _VacancieService;
        public VacancyServices(IVacancieLogic<VacancieModel> VacancieService)
        {
            _VacancieService = VacancieService;
        }
        public IEnumerable<VacancieModel> GetAll()
        {
            return _VacancieService.GetAll().OrderByDescending(x => x.Pinned);
        }
        public async Task<VacancieModel> GetVacancie(int id)
        {
            return await _VacancieService.Get(id);
        }
        public async Task CreateVacancie(VacancieModel Vacancie)
        {
            await _VacancieService.Create(Vacancie);
        }
        public async Task UpdateVacancie(VacancieModel Vacancie)
        {
            await _VacancieService.Update(Vacancie);
        }
        public async Task DeleteVacancie(int id)
        {
            VacancieModel Vacancie = await _VacancieService.Get(id);
            await _VacancieService.Delete(Vacancie);
            await _VacancieService.SaveChanges();
        }

        public IEnumerable<VacancieModel> GetForCompany(int companyId)
        {
            return _VacancieService.GetAll().Where(x => x.CompanyId == companyId);
        }
    }
}
