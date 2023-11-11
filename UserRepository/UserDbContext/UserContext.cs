using Microsoft.EntityFrameworkCore;
using System.Data;
using UserDomain.Map;
using UserDomain.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace UserRepository.UserDbContext
{
    public class UserContext : DbContext
    {
        public DbSet<CvModel> Cv { get; set; }
        public DbSet<RoleModel> RoleModel { get; set; }
        public DbSet<UserModel> User { get; set; }
        public DbSet<LanguageModel> Language { get; set; }
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            RoleModel adminRoleModel = new RoleModel { Id = 1, RoleName = "Admin" };
            RoleModel moderRoleModel = new RoleModel { Id = 2, RoleName = "Moder" };
            RoleModel userRoleModel = new RoleModel { Id = 3, RoleName = "User" };
            RoleModel Seller = new RoleModel { Id = 4, RoleName = "Seller" };
            modelBuilder.Entity<RoleModel>().HasData(new RoleModel[] { adminRoleModel, moderRoleModel, userRoleModel, Seller });
            base.OnModelCreating(modelBuilder);
        }
    }
}
