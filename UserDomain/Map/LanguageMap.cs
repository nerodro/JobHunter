using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserDomain.Models;

namespace UserDomain.Map
{
    public class LanguageMap
    {
        public LanguageMap(EntityTypeBuilder<LanguageModel> entityTypeBuilder) 
        { 
            entityTypeBuilder.HasKey(x => x.Id);
            entityTypeBuilder.Property(x => x.Language).IsRequired();
        }
    }
}
