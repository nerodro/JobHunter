using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.Design;
using VacancieAPI.ViewModel;
using VacancieDomain.Model;
using VacancieService.VacancieService;

namespace VacancieAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VacancieController : ControllerBase
    {
        private readonly IVacancieService _VacancieService;
        public VacancieController(IVacancieService VacancieService)
        {
            _VacancieService = VacancieService;
        }
        [HttpPost("CreateVacancie")]
        public async Task<IActionResult> CreateVacancie(VacancieViewModel model)
        {
            VacancieModel language = new VacancieModel
            {
                CityId = model.CityId,
                CountryId = model.CountryId,
                AboutWork = model.AboutWork.Trim(),
                WorkName = model.WorkName.Trim(),
                CompanyId = model.CompanyId,
            };
            if (model.CompanyId != 0 && model.WorkName != null)
            {
                await _VacancieService.CreateVacancie(language);
                return CreatedAtAction("SingleVacancie", new { id = language.Id }, model);
            }
            return BadRequest("Не все обязательные поля были заполнены");
        }
        [HttpPut("EditVacancie/{id}")]
        public async Task<ActionResult<VacancieViewModel>> EditVacancie(int id, VacancieViewModel model)
        {
            VacancieModel Vacancie = await _VacancieService.GetVacancie(id);
            if (ModelState.IsValid)
            {
                Vacancie.CityId = model.CityId;
                Vacancie.CountryId = model.CountryId;
                Vacancie.AboutWork = model.AboutWork.Trim();
                Vacancie.WorkName = model.WorkName.Trim();
                Vacancie.CompanyId = model.CompanyId;
                if (model.CompanyId != 0 && model.WorkName != null)
                {
                    await _VacancieService.UpdateVacancie(Vacancie);
                    return Ok(model);
                }
            }
            return BadRequest(ModelState);
        }
        [HttpDelete("DeleteVacancie/{id}")]
        public async Task<ActionResult<VacancieViewModel>> DeleteVacancie(int id)
        {
            await _VacancieService.DeleteVacancie(id);
            return Ok("Вакансия успешно удалена");
        }
        [HttpGet("GetOneVacancie/{id}")]
        public async Task<ActionResult<VacancieViewModel>> SingleVacancie(int id)
        {
            VacancieViewModel model = new VacancieViewModel();
            if (id != 0)
            {
                VacancieModel Vacancie = await _VacancieService.GetVacancie(id);
                if (Vacancie != null)
                {

                    model.CityId = Vacancie.CityId;
                    model.CompanyId = Vacancie.CompanyId;
                    model.CountryId = Vacancie.CountryId;
                    model.AboutWork = model.AboutWork.Trim();
                    model.WorkName = model.WorkName.Trim();
                    model.Id = Vacancie.Id;
                    return new ObjectResult(model);
                }
                return BadRequest("Вакансия не найдена");
            }
            return BadRequest();
        }
        [HttpGet("GetAllVacancie")]
        public IEnumerable<VacancieViewModel> Index()
        {
            List<VacancieViewModel> model = new List<VacancieViewModel>();
            if (_VacancieService != null)
            {
                _VacancieService.GetAll().ToList().ForEach(u =>
                {
                    VacancieViewModel Vacancie = new VacancieViewModel
                    {
                        Id = u.Id,
                        CityId = u.CityId,
                        CompanyId = u.CompanyId,
                        CountryId = u.CountryId,
                        AboutWork = u.AboutWork,
                        WorkName = u.WorkName,
                    };
                    model.Add(Vacancie);
                });
            }
            return model;
        }
    }
}
