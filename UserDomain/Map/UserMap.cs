﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserDomain.Models;

namespace UserDomain.Map
{
    public class UserMap
    {
        public UserMap(EntityTypeBuilder<UserModel> entityTypeBuilder) 
        { 
            entityTypeBuilder.HasKey(x => x.Id);
            entityTypeBuilder.Property(x => x.Name).IsRequired();
            entityTypeBuilder.Property(x => x.Surname).IsRequired();
            entityTypeBuilder.Property(x => x.Patronomyc).IsRequired();
            entityTypeBuilder.Property(x => x.Email).IsRequired();
            entityTypeBuilder.Property(x => x.Phone).IsRequired();
            entityTypeBuilder.Property(x => x.CityId).IsRequired();
            entityTypeBuilder.Property(x=> x.CountryId).IsRequired();
            entityTypeBuilder.Property(x => x.Password).IsRequired();
            entityTypeBuilder.Property(x => x.RoleId).IsRequired();
        }
    }
}
