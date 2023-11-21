using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocationDomain.Model
{
    public class CountryModel
    {
        public int Id { get; set; }
        public string CountryName { get; set; } = null!;
        public virtual List<CityModel> City { get; set; }
        public CountryModel() 
        {
            City = new List<CityModel>();
        }
    }
}
