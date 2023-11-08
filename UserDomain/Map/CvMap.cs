using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            entityTypeBuilder.Property(x => x.UserId).IsRequired();
        }
    }
}
