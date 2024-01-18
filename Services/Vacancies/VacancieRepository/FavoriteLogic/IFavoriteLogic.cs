using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacancieDomain.Model;

namespace VacancieRepository.FavoriteLogic
{
    public interface IFavoriteLogic<T> where T : FavoriteVacancie
    {
        IEnumerable<T> GetAllForUser(int Id);
        Task<T> Get(int id);
        IEnumerable<T> GetAll();
        Task Create(T entity);
        Task Delete(T entity);
        void Remove(T entity);
        Task SaveChanges();
    }
}
