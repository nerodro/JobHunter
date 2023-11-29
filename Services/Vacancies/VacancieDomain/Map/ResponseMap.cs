using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacancieDomain.Model;

namespace VacancieDomain.Map
{
    public class ResponseMap
    {
        public ResponseMap(EntityTypeBuilder<ResponseModel> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(x => x.Id);
            entityTypeBuilder.Property(x => x.CvId).IsRequired();
            entityTypeBuilder.Property(x => x.VacancieId).IsRequired();
            entityTypeBuilder.Property(x => x.Message).IsRequired();
        }
    }
}
