using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacancieDomain.Model;

namespace VacancieService.Favorite
{
    public interface IFavoriteService
    {
        IEnumerable<FavoriteVacancie> GetAll();
        IEnumerable<FavoriteVacancie> GetAllForUser(int Id);
        Task CreateFavorite(FavoriteVacancie Vacancie);
        Task DeleteFavorite(int id);
    }
}
