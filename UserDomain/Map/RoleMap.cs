using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserDomain.Models;

namespace UserDomain.Map
{
    public class RoleMap
    {
        public RoleMap(EntityTypeBuilder<RoleModel> entityTypeBuilder) 
        {
            entityTypeBuilder.HasKey(x => x.Id);
            entityTypeBuilder.Property(x => x.RoleName).IsRequired();
        }
    }
}
