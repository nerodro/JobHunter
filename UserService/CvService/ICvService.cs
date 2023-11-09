using UserDomain.Models;

namespace UserService.CvService
{
    public interface ICvService
    {
        IEnumerable<CvModel> GetAll();
        CvModel GetCV(int id);
        void Create(CvModel cv);
        void Update(CvModel cv);
        void DeleteOfUser(int id);
        void Delete(int id);
    }
}
