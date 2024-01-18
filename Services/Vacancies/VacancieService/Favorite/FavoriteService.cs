using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacancieDomain.Model;
using VacancieRepository.FavoriteLogic;
using VacancieRepository.ResponseLogic;

namespace VacancieService.Favorite
{
    public class FavoriteService : IFavoriteService
    {
        private readonly IFavoriteLogic<FavoriteVacancie> _FavoriteLogic;
        public FavoriteService(IFavoriteLogic<FavoriteVacancie> favoriteLogic)
        {
            _FavoriteLogic = favoriteLogic;
        }

        public async Task CreateFavorite(FavoriteVacancie Vacancie)
        {
            await _FavoriteLogic.Create(Vacancie);
        }

        public async Task DeleteFavorite(int id)
        {
            FavoriteVacancie favorite = await _FavoriteLogic.Get(id);
            await _FavoriteLogic.Delete(favorite);
            await _FavoriteLogic.SaveChanges();
        }

        public IEnumerable<FavoriteVacancie> GetAll()
        {
            return _FavoriteLogic.GetAll();
        }

        public IEnumerable<FavoriteVacancie> GetAllForUser(int Id)
        {
            return _FavoriteLogic.GetAllForUser(Id);
        }
    }
}
