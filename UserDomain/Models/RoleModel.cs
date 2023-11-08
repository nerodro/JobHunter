using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserDomain.Models
{
    public class RoleModel
    {
        public int Id { get; set; }
        public string RoleName { get; set; } = null!;
        public virtual List<UserModel> Users { get; set; }
        public RoleModel() 
        { 
            Users = new List<UserModel>();
        }
    }
}
