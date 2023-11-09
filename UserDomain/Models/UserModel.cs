namespace UserDomain.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string Patronomyc { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int Phone { get; set; }
        public int CityId { get; set; }
        public int CountryId { get; set; }
        public int RoleId { get; set; }
        public virtual RoleModel RoleModel { get; set; }
        public virtual List<CvModel> CvModel { get; set; }
        public UserModel()
        {
            CvModel = new List<CvModel>();
            RoleModel = new RoleModel();
        }
    }
}
