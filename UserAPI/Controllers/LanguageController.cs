using Microsoft.AspNetCore.Mvc;
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

    }
}
