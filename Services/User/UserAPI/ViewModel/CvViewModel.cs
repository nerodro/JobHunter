using Microsoft.AspNetCore.Mvc;
using UserDomain.Models;

namespace UserAPI.ViewModel
{
    public class CvViewModel
    {
        [HiddenInput]
        public Int64 Id { get; set; }
        public string JobNmae { get; set; } = null!;
        public string AboutMe { get; set; } = null!;
        public int UserId { get; set; }
        public string UserName { get; set; } = null!;
        public int LanguageId { get; set; }
        public string LanguageName { get; set; } = null!;
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
    }
}
