using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserDomain.Models
{
    public class CountryModel
    {
        public int Id { get; set; }
        public string CountryName { get; set; } = null!;
        public virtual List<UserModel> Users { get; set; }
        public CountryModel() 
        { 
            Users = new List<UserModel>();
        }
    }
}
