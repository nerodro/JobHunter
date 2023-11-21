using Microsoft.AspNetCore.Mvc;

namespace LocationAPI.ViewModel
{
    public class CityViewModel
    {
        [HiddenInput]
        public Int64 Id { get; set; }
        public string CityName { get; set; } = null!;
        public int CountryId { get; set; }
        public string CountryName { get; set; } = null!;
    }
}
