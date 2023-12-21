using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacancieDomain.Model;
using VacancieRepository.ResponseLogic;
using VacancieService.VacancieService;

namespace VacancieService.ResponseService
{
    public class ResponseService : IResponseService
    {
        private readonly IResponseLogic<ResponseModel> _ResponseService;
        public ResponseService(IResponseLogic<ResponseModel> ResponseService)
        {
            _ResponseService = ResponseService;
        }
        public IEnumerable<ResponseModel> GetAll()
        {
            return _ResponseService.GetAll();
        }
        public async Task<ResponseModel> GetResponse(int id)
        {
            return await _ResponseService.Get(id);
        }
        public async Task CreateResponse(ResponseModel Response)
        {
            await _ResponseService.Create(Response);
        }
        public async Task UpdateResponse(ResponseModel Response)
        {
            await _ResponseService.Update(Response);
        }
        public async Task DeleteResponse(int id)
        {
            ResponseModel Response = await _ResponseService.Get(id);
            await _ResponseService.Delete(Response);
            await _ResponseService.SaveChanges();
        }

        public IEnumerable<ResponseModel> GetForCv(int CvId)
        {
            return _ResponseService.GetAll().Where(x => x.CvId == CvId);
        }
    }
}
