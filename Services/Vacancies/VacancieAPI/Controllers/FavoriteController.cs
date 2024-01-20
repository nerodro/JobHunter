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
                UserId = await GetUserId(model.UserId)
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
        private async Task<int> GetUserId(int id)
        {
            var user = await _userRpc.GetUser(id);
            return (int)user.Id;
        }
    }
}
