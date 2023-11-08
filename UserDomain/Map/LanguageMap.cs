using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
