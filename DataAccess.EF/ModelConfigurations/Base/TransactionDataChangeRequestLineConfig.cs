using Common.Enums;
using DataAccess.Models.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.EF.ModelConfigurations.Base
{
    abstract public class TransactionDataChangeRequestLineConfig : ChangeRequestLineConfig
    {
        public void Configure<T>(EntityTypeBuilder<T> builder) where T : TransactionDataChangeRequestLine
        {
           base.Configure(builder);

            builder
              .Property(x => x.Amount)
              .HasDefaultValue(0d)
              .IsRequired();

            builder
                .Property(x => x.Date)
                .IsRequired();

            builder
              .Property(x => x.Sequence)
              .IsRequired();

            builder
              .Property(x => x.Reference)
              .IsRequired();
        }
    }
}
