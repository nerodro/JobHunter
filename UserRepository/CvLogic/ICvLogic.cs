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
        T Get(long id);
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Remove(T entity);
        void SaveChanges();
    }
}
