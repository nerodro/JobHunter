using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace CompanyAPI.ViewModel
{
    public class LoginViewModel
    {
        public string FirstName { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
