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
    public abstract class ChangeRequestLineConfig : EntityBaseConfig
    {
        protected void Configure<T>(EntityTypeBuilder<T> builder) where T : ChangeRequestLine
        {
            base.Configure(builder);
            builder
                .Property(b => b.RequestState)
                .HasDefaultValue(ChangeRequestState.PENDING)
                .IsRequired();

        }
    }
}
