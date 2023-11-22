using Microsoft.AspNetCore.Mvc;

namespace UserAPI.ViewModel
{
    public class CountryViewModel
    {
        [HiddenInput]
        public Int64 Id { get; set; }
        public string CountryName { get; set; } = null!;
    }
}
