using CategoryDomain.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CategoryDomain.Map
{
    public class CategoryMap
    {
        public CategoryMap(EntityTypeBuilder<CategoryModel> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(x => x.Id);
            entityTypeBuilder.Property(x => x.CategoryName).IsRequired();
        }
    }
}
