using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserDomain.Models;
using UserRepository.LanguageLogic;
using UserRepository.UserLogic;

namespace UserService.LanguageService
{
    public class LanguageService : ILanguageService
    {
        private ILanguageLogic<LanguageModel> _language;
        public LanguageService(ILanguageLogic<LanguageModel> language)
        {
            _language = language;
        }
        public IEnumerable<LanguageModel> GetAll()
        {
            return _language.GetAll();
        }
        public async Task<LanguageModel> GetLanguage(int id)
        {
            return await _language.Get(id);
        }
        public async Task Create(LanguageModel user)
        {
            await _language.Create(user);
        }
        public async Task Update(LanguageModel user)
        {
            await _language.Update(user);
        }
        public async Task Delete(int id)
        {
            LanguageModel user = await GetLanguage(id);
            await _language.Delete(user);
            await _language.SaveChanges();
        }
    }
}
