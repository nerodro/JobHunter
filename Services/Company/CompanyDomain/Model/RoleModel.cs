using CompanyDomain.Model;

namespace UserDomain.Models
{
    public class RoleModel
    {
        public int Id { get; set; }
        public string RoleName { get; set; } = null!;
        public virtual List<CompanyModel> Company { get; set; }
        public RoleModel() 
        { 
            Company = new List<CompanyModel>();
        }
    }
}
