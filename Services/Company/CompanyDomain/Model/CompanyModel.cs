using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyDomain.Model
{
    public class CompanyModel
    {
        public int Id { get; set; }
        public string CompanyName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int Phone { get; set; }
        public int CityId { get; set; }
        public int CountryId { get; set; }
        public int CategoryId { get; set; }
    }
}
