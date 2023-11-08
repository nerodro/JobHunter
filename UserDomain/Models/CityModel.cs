using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserDomain.Models
{
    public class CityModel
    {
        public int Id { get; set; }
        public string CityName { get; set; } = null!;
        public virtual List<CityModel> City { get; set; }
        public CityModel() 
        {
            City = new List<CityModel>();
        }
    }
}
