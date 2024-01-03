using CategoryDomain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CategoryRepository
{
    public class CategoryContext : DbContext
    {
        public DbSet<CategoryModel> Category { get; set; }
        public CategoryContext(DbContextOptions<CategoryContext> options) : base(options)
        {
            //Database.Migrate();
            //Database.EnsureDeleted();
            //Database.EnsureCreated();
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseNpgsql("Server=localhost;Port=5432;Database=Category;Username=postgres;Password=123;");
        //}
    }
}

