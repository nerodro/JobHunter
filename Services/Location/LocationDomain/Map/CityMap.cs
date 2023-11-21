using LocationDomain.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LocationDomain.Map
{
    public class CityMap
    {
        public CityMap(EntityTypeBuilder<CityModel> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(x => x.Id);
            entityTypeBuilder.Property(x => x.CityName).IsRequired();
            entityTypeBuilder.Property(x => x.CountryId).IsRequired();
        }
    }
}
