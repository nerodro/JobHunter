using UserDomain.Models;
using UserRepository.CvLogic;
using UserRepository.UserDbContext;

namespace UserService.CvService
{
    public class CvService : ICvService
    {
        private ICvLogic<CvModel> _cvService;
        private readonly UserContext _userContext;
        public CvService(ICvLogic<CvModel> cvService, UserContext userContext)
        {
            _cvService = cvService;
            _userContext = userContext;
        }
        public IAsyncEnumerable<CvModel> GetAll()
        {
            return _cvService.GetAll();
        }
        public async Task<CvModel> GetCV(int id)
        {
            return await _cvService.Get(id);
        }
        public async Task Create(CvModel cv)
        {
            await _cvService.Create(cv);
        }
        public async Task Update(CvModel cv)
        {
            await _cvService.Update(cv);
        }
        public async Task Delete(int id)
        {
            CvModel cv = await GetCV(id);
            await _cvService.Delete(cv);
            await _cvService.SaveChanges();
        }

        public void DeleteOfUser(int id)
        {
            List<CvModel> cvModels =  _userContext.Cv.Where(x => x.UserId == id).ToList();
            _cvService.DeleteOfUser(cvModels);
        }
    }
}
