using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacancieDomain.Model;

namespace VacancieRepository
{
    public class VacancyContext : DbContext
    {
        public DbSet<ResponseModel> Responses { get; set; }
        public DbSet<VacancieModel> Vacanies { get; set;}
        public DbSet<FavoriteVacancie> Favorites { get; set; }
        public VacancyContext(DbContextOptions<VacancyContext> options) : base(options)
        {
            // Database.Migrate();
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=Vacancy;Username=postgres;Password=123;");
        //}
    }
}
