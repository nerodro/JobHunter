using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserDomain.Models;

namespace UserService.LanguageService
{
    public interface ILanguageService
    {
        IEnumerable<LanguageModel> GetAll();
        Task<LanguageModel> GetLanguage(int id);
        Task Create(LanguageModel model);
        Task Update(LanguageModel model);
        Task Delete(int id);
    }
}
