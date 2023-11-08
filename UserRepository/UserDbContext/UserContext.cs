using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserDomain.Models;

namespace UserRepository.UserDbContext
{
    public class UserContext : DbContext
    {
        public DbSet<CityModel> Cities { get; set; }
        public DbSet<CountryModel> Country { get; set; }
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
