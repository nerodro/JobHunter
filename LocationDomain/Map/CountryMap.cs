using LocationDomain.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LocationDomain.Map
{
    public class CountryMap
    {
        public CountryMap(EntityTypeBuilder<CountryModel> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(x => x.Id);
            entityTypeBuilder.Property(x => x.CountryName).IsRequired();
        }
    }
}
}
