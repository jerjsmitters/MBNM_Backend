using Common.Enums;
using DataAccess.Models.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
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

            //required as oracles mysql package is v broken and Pomelo is yet to be ported to dotnet 10
            var dateConverter = new ValueConverter<DateOnly, DateTime>(
                d => d.ToDateTime(TimeOnly.MinValue),
                dt => DateOnly.FromDateTime(dt));

            builder
                .Property(x => x.Date)
                .HasConversion(dateConverter)
                .IsRequired();

            builder
              .Property(x => x.Sequence)
              .IsRequired();

            builder
              .Property(x => x.Reference)
              .IsRequired();

            builder.HasMany(x => x.TransactionProperties);
            //builder.Navigation(b => b.TransactionProperties).AutoInclude();

            builder.HasMany(x => x.TransactionCategories);
            //builder.Navigation(b => b.TransactionCategories).AutoInclude();


        }
    }
}
