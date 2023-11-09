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
        public IEnumerable<CvModel> GetAll()
        {
            return _cvService.GetAll();
        }
        public CvModel GetCV(int id)
        {
            return _cvService.Get(id);
        }
        public void Create(CvModel cv)
        {
            _cvService.Create(cv);
        }
        public void Update(CvModel cv)
        {
            _cvService.Update(cv);
        }
        public void Delete(int id)
        {
            CvModel cv = GetCV(id);
            _cvService.Delete(cv);
            _cvService.SaveChanges();
        }

        public void DeleteOfUser(int id)
        {
            List<CvModel> cvModels = _userContext.Cv.Where(x => x.UserId == id).ToList();
            _cvService.DeleteOfUser(cvModels);
        }
    }
}
