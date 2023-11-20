using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocationDomain.Model
{
    public class CityModel
    {
        public int Id { get; set; }
        public string CityName { get; set; } = null!;
        public int CountryId { get; set; }
        public virtual CountryModel Country { get; set; }
    }
}
