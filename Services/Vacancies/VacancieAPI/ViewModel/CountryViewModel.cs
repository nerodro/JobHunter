using Microsoft.AspNetCore.Mvc;

namespace VacancieAPI.ViewModel
{
    public class CountryViewModel
    {
        [HiddenInput]
        public long Id { get; set; }
        public string CountryName { get; set; } = null!;
    }
}
