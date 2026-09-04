using DataAccess.Models.Base;
using DataAccess.Models.ChangeRequestEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.EF.ModelConfigurations.Base
{
    internal abstract class EntityTypeConfigurationBase<T> : IEntityTypeConfiguration<T> where T : EntityBase
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            //inherited
            builder.HasKey(b => b.Id);

            builder
                .Property(b => b.CreatedOn)
                .IsRequired();
        }
    }
}
