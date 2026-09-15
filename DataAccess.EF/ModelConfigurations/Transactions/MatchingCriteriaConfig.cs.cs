using DataAccess.EF.ModelConfigurations.Base;
using DataAccess.Models.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.EF.ModelConfigurations.Transactions
{
    internal class MatchingCriteriaConfig : IEntityTypeConfiguration<MatchingCriteria>
    {
        public void Configure(EntityTypeBuilder<MatchingCriteria> builder)
        {
            EntityBaseConfig.Configure(builder);

            builder.Property(b => b.SearchText).IsRequired();
            builder.Property(b => b.MatchingCriteriaMode).IsRequired();

            builder.HasMany(b => b.TransactionProperties).WithMany(p => p.CriteriaToMatchOn);
            builder.HasMany(b => b.TransactionCategories).WithMany(c => c.CriteriaToMatchOn);
        }
    }
}
