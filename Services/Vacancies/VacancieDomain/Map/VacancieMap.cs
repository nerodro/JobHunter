using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacancieDomain.Model;

namespace VacancieDomain.Map
{
    public class VacancieMap
    {
        public VacancieMap(EntityTypeBuilder<VacancieModel> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(x => x.Id);
            entityTypeBuilder.Property(x => x.WorkName).IsRequired();
            entityTypeBuilder.Property(x => x.AboutWork).IsRequired();
            entityTypeBuilder.Property(x => x.CompanyId).IsRequired();
            entityTypeBuilder.Property(x => x.CityId).IsRequired();
            entityTypeBuilder.Property(x => x.CountryId).IsRequired();
            entityTypeBuilder.Property(x => x.Salary).IsRequired();
            entityTypeBuilder.Property(x => x.Pinned).IsRequired();
        }
    }
}
