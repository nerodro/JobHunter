using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacancieDomain.Model;

namespace VacancieDomain.Map
{
    public class FavoriteMap
    {
        public FavoriteMap(EntityTypeBuilder<FavoriteVacancie> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(x => x.Id);
            entityTypeBuilder.Property(x => x.UserId).IsRequired();
            entityTypeBuilder.Property(x => x.VacancieId).IsRequired();
        }
    }
}
