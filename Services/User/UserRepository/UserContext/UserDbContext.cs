using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserDomain.Map;
using UserDomain.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace UserRepository.UserContext
{
    public class UserDbContext : DbContext
    {
        public DbSet<CvModel> Cv { get; set; }
        public DbSet<RoleModel> Role { get; set; }
        public DbSet<UserModel> User { get; set; }
        public DbSet<LanguageModel> Language { get; set; }
        //public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        //{
        //    Database.Migrate();
        //    Database.EnsureDeleted();
        //    Database.EnsureCreated();
        //}
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=JobUser;Username=postgres;Password=123;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            new UserMap(modelBuilder.Entity<UserModel>());
            new RoleMap(modelBuilder.Entity<RoleModel>());


            RoleModel adminRoleModel = new RoleModel { Id = 1, RoleName = "Admin" };
            RoleModel moderRoleModel = new RoleModel { Id = 2, RoleName = "Moder" };
            RoleModel userRoleModel = new RoleModel { Id = 3, RoleName = "User" };
            modelBuilder.Entity<RoleModel>().HasData(new RoleModel[] { adminRoleModel, moderRoleModel, userRoleModel});
            base.OnModelCreating(modelBuilder);
        }
    }
}
