using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserDomain.Models;

namespace UserRepository.CvLogic
{
    public interface ICvLogic<T> where T : CvModel
    {
        IEnumerable<T> GetAll();
        Task<T> Get(int id);
        Task Create(T entity);
        Task Update(T entity);
        Task Delete(T entity);
        void Remove(T entity);
        Task SaveChanges();
    }
}
