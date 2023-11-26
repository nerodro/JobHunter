using Microsoft.AspNetCore.Mvc;

namespace CompanyAPI.ViewModel
{
    public class CompanyViewModel
    {
        [HiddenInput]
        public Int64 Id { get; set; }
        public string CompanyName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int Phone { get; set; }
        public int CityId { get; set; }
        public int CountryId { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
        public string CityName { get; set; } = null!;   
        public string CountryName { get; set; } = null!;

    }
}
