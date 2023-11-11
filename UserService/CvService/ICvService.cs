using UserDomain.Models;

namespace UserService.CvService
{
    public interface ICvService
    {
        IAsyncEnumerable<CvModel> GetAll();
        Task<CvModel> GetCV(int id);
        Task Create(CvModel cv);
        Task Update(CvModel cv);
        void DeleteOfUser(int id);
        Task Delete(int id);
    }
}
