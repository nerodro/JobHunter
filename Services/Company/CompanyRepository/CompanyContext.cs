using CompanyDomain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyRepository
{
    public class CompanyContext : DbContext
    {
        public DbSet<CompanyModel> Companie { get; set; }
        public CompanyContext(DbContextOptions<CompanyContext> options) : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=Company;Username=postgres;Password=123;");
        //}
    }
}
