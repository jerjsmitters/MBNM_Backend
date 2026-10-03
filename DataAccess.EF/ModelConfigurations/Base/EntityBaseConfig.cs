using DataAccess.Models.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.EF.ModelConfigurations.Base
{
    public abstract class EntityBaseConfig
    {
        protected void Configure<T>(EntityTypeBuilder<T> builder) where T : EntityBase
        {
            builder.HasKey(b => b.Id);
            builder.Property(b => b.CreatedOn).IsRequired();
            builder.Property(b => b.IsDeleted).IsRequired().HasDefaultValue(false);
        }
    }
}
