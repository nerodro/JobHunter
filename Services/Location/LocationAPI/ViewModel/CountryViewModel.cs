using Microsoft.AspNetCore.Mvc;

namespace LocationAPI.ViewModel
{
    public class CountryViewModel
    {
        [HiddenInput]
        public Int64 Id { get; set; }
        public string CountryName { get; set; } = null!;
    }
}
