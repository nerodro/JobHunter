using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using UserAPI.ViewModel;
using UserDomain.Models;
using UserService.LanguageService;

namespace UserAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LanguageController : ControllerBase
    {
        private readonly ILanguageService _language;
        public LanguageController(ILanguageService language)
        {
            _language = language;
        }
        [HttpPost("CreateLanguage")]
        public async Task<IActionResult> CreateLanguage(LanguageViewModel model)
        {
            LanguageModel language = new LanguageModel
            {
                Language = model.LanguageName.Trim()
            };
            if (model.LanguageName != null)
            {
                await _language.Create(language);
                return CreatedAtAction("SingleLanguage", new { id = language.Id }, model);
            }
            return BadRequest("Не все обязательные поля были заполнены");
        }
        [HttpPut("EditLanguage/{id}")]
        public async Task<IActionResult> EditLanguage(int id, LanguageViewModel model)
        {
            LanguageModel language = await _language.GetLanguage(id);
            if (ModelState.IsValid)
            {
                language.Language = model.LanguageName.Trim();
                if (model.LanguageName != null)
                {
                    await _language.Update(language);
                    return Ok(model);
                }
            }
            return BadRequest(ModelState);
        }
        [HttpDelete("DeleteLanguage/{id}")]
        public async Task<ActionResult<LanguageViewModel>> DeleteLanguage(int id)
        {
            var model = await _language.GetLanguage(id);
            if (model != null)
            {
                await _language.Delete(id);
                return Ok("Язык успешно удален");
            }
            return BadRequest();
        }
        [HttpGet("GetOneLanguage/{id}")]
        public async Task<IActionResult> SingleLanguage(int id)
        {
            LanguageViewModel model = new LanguageViewModel();
            if (id != 0)
            {
                LanguageModel language = await _language.GetLanguage(id);
                if (language != null)
                {
                    model.LanguageName = language.Language;
                    model.Id = language.Id;
                    return new ObjectResult(model);
                }
                return BadRequest("Язык не найден");
            }
            return BadRequest();
        }
        [HttpGet("GetAllLanguage")]
        public IEnumerable<LanguageViewModel> Index()
        {
            List<LanguageViewModel> model = new List<LanguageViewModel>();
            if (_language != null)
            {
                _language.GetAll().ToList().ForEach(u =>
                {
                    LanguageViewModel language = new LanguageViewModel
                    {
                        Id = u.Id,
                        LanguageName = u.Language
                    };
                    model.Add(language);
                });
            }
            return model;
        }
    }
}
