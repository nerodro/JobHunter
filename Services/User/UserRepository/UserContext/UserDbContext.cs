using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserDomain.Models;

namespace UserRepository.UserContext
{
    public class UserDbContext : DbContext
    {
        public DbSet<CvModel> Cv { get; set; }
        public DbSet<RoleModel> Role { get; set; }
        public DbSet<UserModel> User { get; set; }
        public DbSet<LanguageModel> Language { get; set; }
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=JobUser;Username=postgres;Password=123;");
        //}
    }
}
