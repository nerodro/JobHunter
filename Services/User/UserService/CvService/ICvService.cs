using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserDomain.Models;

namespace UserService.CvService
{
    public interface ICvService
    {
        IEnumerable<CvModel> GetAll();
        Task<CvModel> GetCv(int id);
        Task CreateCv(CvModel cv);
        Task UpdateCv(CvModel cv);
        Task DeleteCv(int id);
    }
}
