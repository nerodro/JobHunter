using LocationDomain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocationRepository
{
    public class LocationContext : DbContext
    {
        public DbSet<CountryModel> Countries { get; set; }
        public DbSet<CityModel> Cities { get; set; }
        public LocationContext(DbContextOptions<LocationContext> options) : base(options)
        {
            //Database.Migrate();
            //Database.EnsureDeleted();
            //Database.EnsureCreated();
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=Location;Username=postgres;Password=123;");
        //}
    }
}
