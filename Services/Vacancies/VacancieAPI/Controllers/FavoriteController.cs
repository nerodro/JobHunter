using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VacancieAPI.VacancieRpc;
using VacancieAPI.ViewModel;
using VacancieDomain.Model;
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
        [HttpPost("CreateFavorite")]
        [Authorize(Roles = "Admin,Moder,User")]
        public async Task<IActionResult> CreateResponse(FavoriteViewModel model)
        {
            FavoriteVacancie favorite = new FavoriteVacancie
            {
                VacancieId = model.VacancieId,
                UserId = GetUserId(model.UserId)
            };
            if (model.VacancieId != 0 && model.UserId != 0)
            {
                await _favorite.CreateFavorite(favorite);
                return Ok();
            }
            return BadRequest("Не все обязательные поля были заполнены");
        }
        [HttpDelete("DeleteFavorite/{id}")]
        [Authorize(Roles = "Admin,Moder,User")]
        public async Task<ActionResult<FavoriteViewModel>> DeleteFavorite(int id)
        {
            await _favorite.DeleteFavorite(id);
            return Ok("Вакансия удалена из избранной");
        }
        [HttpGet("GetAllFavorite")]
        [Authorize(Roles = "Admin,Moder")]
        public IEnumerable<FavoriteViewModel> Index()
        {
            List<FavoriteViewModel> model = new List<FavoriteViewModel>();
            if (_favorite != null)
            {
                _favorite.GetAll().ToList().ForEach(u =>
                {
                    FavoriteViewModel favorite = new FavoriteViewModel
                    {
                        Id = u.Id,
                        UserId = GetUserId(u.UserId),
                        VacancieId = u.VacancieId,
                    };
                    model.Add(favorite);
                });
            }
            return model;
        }
        [HttpGet("GetAllFavoriteForUser")]
        [Authorize(Roles = "Admin,Moder,User")]
        public IEnumerable<FavoriteViewModel> GetForUser(int id)
        {
            List<FavoriteViewModel> model = new List<FavoriteViewModel>();
            if (_favorite != null)
            {
                _favorite.GetAllForUser(id).ToList().ForEach(u =>
                {
                    FavoriteViewModel favorite = new FavoriteViewModel
                    {
                        Id = u.Id,
                        UserId = GetUserId(u.UserId),
                        VacancieId = u.VacancieId,
                    };
                    model.Add(favorite);
                });
            }
            return model;
        }
        private int GetUserId(int id)
        {
            var user = _userRpc.GetUser(id);
            return (int)user.Id;
        }
    }
}
