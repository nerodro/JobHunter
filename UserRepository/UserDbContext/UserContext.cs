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
        public DbSet<CityModel>
    }
}
