using Microsoft.AspNetCore.Mvc;

namespace VacancieAPI.ViewModel
{
    public class VacancieViewModel
    {
        [HiddenInput]
        public Int64 Id { get; set; }
        public string WorkName { get; set; } = null!;
        public string AboutWork { get; set; } = null!;
        public int CompanyId { get; set; }
        public int CityId { get; set; }
        public int CountryId { get; set; }
    }
}
