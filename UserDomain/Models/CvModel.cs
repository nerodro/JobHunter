namespace UserDomain.Models
{
    public class CvModel
    {
        public int Id { get; set; }
        public string JobNmae { get; set; } = null!;
        public string AboutMe { get; set; } = null!;
        public int UserId { get; set; }
        public virtual UserModel User { get; set; }
        public int LanguageId { get; set; }
        public virtual LanguageModel Language { get; set; }
        public CvModel() 
        {
            User = new UserModel();
            Language = new LanguageModel();
        }
    }
}
