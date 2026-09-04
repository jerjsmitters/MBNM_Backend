using Common.Enums;
using DataAccess.Models.Base;
using DataAccess.Models.ChangeRequestEntities.Line;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.EF.ModelConfigurations.Base
{
    internal abstract class ChangeRequestLineConfig<T> : EntityTypeConfigurationBase<T> where T : ChangeRequestLine
    {
        new public void Configure(EntityTypeBuilder<T> builder)
        {
            builder
                .Property(b => b.RequestState)
                .HasDefaultValue(ChangeRequestState.PENDING)
                .IsRequired();

            base.Configure(builder);
        }
    }
}
