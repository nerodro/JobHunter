using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserDomain.Models;

namespace UserDomain.Map
{
    public class CvMap
    {
        public CvMap(EntityTypeBuilder<CvModel> entityTypeBuilder) 
        { 
            entityTypeBuilder.HasKey(x => x.Id);
            entityTypeBuilder.Property(x => x.JobNmae).IsRequired();
            entityTypeBuilder.Property(x => x.AboutMe).IsRequired();
            entityTypeBuilder.Property(x => x.LanguageId).IsRequired();
            entityTypeBuilder.Property(x => x.CategoryId).IsRequired();
        }
    }
}
