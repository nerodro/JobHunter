using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserDomain.Models;
using UserRepository.CvLogic;

namespace UserService.CvService
{
    public class CvService : ICvService
    {
        private readonly ICvLogic<CvModel> _cvService;
        public CvService(ICvLogic<CvModel> cvService)
        {
            _cvService = cvService;
        }
        public IEnumerable<CvModel> GetAll()
        {
            return _cvService.GetAll();
        }
        public async Task<CvModel> GetCv(int id)
        {
            return await _cvService.Get(id);
        }
        public async Task CreateCv(CvModel cv)
        {
            await _cvService.Create(cv);
        }
        public async Task UpdateCv(CvModel cv)
        {
            await _cvService.Update(cv);
        }
        public async Task DeleteCv(int id)
        {
            CvModel cv = await _cvService.Get(id);
            await _cvService.Delete(cv);
            await _cvService.SaveChanges();
        }
    }
}
