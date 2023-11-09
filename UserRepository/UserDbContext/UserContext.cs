using Microsoft.EntityFrameworkCore;
using UserDomain.Models;

namespace UserRepository.UserDbContext
{
    public class UserContext : DbContext
    {
        public DbSet<CvModel> Cv { get; set; }
        public DbSet<RoleModel> Role { get; set; }
        public DbSet<UserModel> User { get; set; }
        public DbSet<LanguageModel> Language { get; set; }
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
