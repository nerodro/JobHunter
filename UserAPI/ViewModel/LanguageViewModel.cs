using Microsoft.AspNetCore.Mvc;

namespace UserAPI.ViewModel
{
    public class LanguageViewModel
    {
        [HiddenInput]
        public Int64 Id { get; set; }
        public string LanguageName { get; set; } = null!;
    }
}
