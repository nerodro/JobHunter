using Microsoft.AspNetCore.Mvc;

namespace UserAPI.ViewModel
{
    public class UserViewModel
    {
        [HiddenInput]
        public Int64 Id { get; set; }
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string Patronomyc { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int Phone { get; set; }
        public string CityName { get; set; } = null!; 
        public int CityId { get; set; }
        public string CountryName { get; set; } = null!;
        public int CountryId { get; set; }
        public int RoleId { get; set; }
    }
}
