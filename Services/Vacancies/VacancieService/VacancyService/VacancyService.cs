using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacancieDomain.Model;
using VacancieRepository.VacancieLogic;

namespace VacancieService.VacancyService
{
    public class VacancyService
    {
        private readonly IVacancieLogic<VacancieModel> _VacancieService;
        public VacancyService(IVacancieLogic<VacancieModel> VacancieService)
        {
            _VacancieService = VacancieService;
        }
        public IEnumerable<VacancieModel> GetAll()
        {
            return _VacancieService.GetAll();
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
    }
}
