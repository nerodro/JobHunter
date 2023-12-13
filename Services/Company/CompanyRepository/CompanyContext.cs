using CompanyDomain.Map;
using CompanyDomain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserDomain.Map;
using UserDomain.Models;

namespace CompanyRepository
{
    public class CompanyContext : DbContext
    {
        public DbSet<CompanyModel> Companie { get; set; }
        public DbSet<RoleModel> Role { get; set; }
        public CompanyContext(DbContextOptions<CompanyContext> options) : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=Company;Username=postgres;Password=123;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            new CompanyMap(modelBuilder.Entity<CompanyModel>());
            new RoleMap(modelBuilder.Entity<RoleModel>());


            RoleModel adminRoleModel = new RoleModel { Id = 1, RoleName = "Admin" };
            RoleModel moderRoleModel = new RoleModel { Id = 2, RoleName = "Moder" };
            RoleModel userRoleModel = new RoleModel { Id = 3, RoleName = "Company" };
            modelBuilder.Entity<RoleModel>().HasData(new RoleModel[] { adminRoleModel, moderRoleModel, userRoleModel });
            base.OnModelCreating(modelBuilder);
        }
    }
}
