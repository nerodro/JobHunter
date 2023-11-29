using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacancieDomain.Model;

namespace VacancieService.ResponseService
{
    public interface IResponseService
    {
        IEnumerable<ResponseModel> GetAll();
        Task<ResponseModel> GetResponse(int id);
        Task CreateResponse(ResponseModel Response);
        Task UpdateResponse(ResponseModel Response);
        Task DeleteResponse(int id);
    }
}
