using Microsoft.AspNetCore.Mvc;

namespace VacancieAPI.ViewModel
{
    public class CityViewModel
    {
        [HiddenInput]
        public long Id { get; set; }
        public string CityName { get; set; } = null!;
        public int CountryId { get; set; }
        public string CountryName { get; set; } = null!;
    }
}
