using Microsoft.AspNetCore.Mvc;

namespace CompanyAPI.ViewModel
{
    public class VacancieViewModel
    {
        [HiddenInput]
        public Int64 Id { get; set; }
        public string WorkName { get; set; } = null!;
        public string AboutWork { get; set; } = null!;
        public int CompanyId { get; set; }
        public string CompanyName { get; set; } = null!;
        public int CityId { get; set; }
        public string CityName { get; set; } = null!;
        public int CountryId { get; set; }
        public string CountryName { get; set; } = null!;
        public int Salary { get; set; }
    }
}
