using CompanyDomain.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyDomain.Map
{
    public class CompanyMap
    {
        public CompanyMap(EntityTypeBuilder<CompanyModel> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(x => x.Id);
            entityTypeBuilder.Property(x => x.CompanyName).IsRequired();
            entityTypeBuilder.Property(x => x.CountryId).IsRequired();
            entityTypeBuilder.Property(x => x.CityId).IsRequired();
            entityTypeBuilder.Property(x => x.Email).IsRequired();
            entityTypeBuilder.Property(x => x.Password).IsRequired();
            entityTypeBuilder.Property(x => x.Phone).IsRequired();
        }
    }
}
