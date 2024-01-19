using Microsoft.AspNetCore.Mvc;
using VacancieAPI.VacancieRpc;
using VacancieService.Favorite;

namespace VacancieAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FavoriteController : ControllerBase
    {
        private readonly IFavoriteService _favorite;
        private readonly UserRpc _userRpc;
        public FavoriteController(IFavoriteService favorite, UserRpc user)
        {
            _favorite = favorite;
            _userRpc = user;
        }
    }
}
