using DataAccess.EF.ModelConfigurations.Base;
using DataAccess.Models.Criteria;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.EF.ModelConfigurations.Transactions
{
    internal class MatchingCriteriaConfig : EntityBaseConfig, IEntityTypeConfiguration<MatchingCriteria>
    {
        public void Configure(EntityTypeBuilder<MatchingCriteria> builder)
        {
            base.Configure(builder);

            builder.Property(b => b.SearchText).IsRequired();
            builder.Property(b => b.MatchingCriteriaMode).IsRequired();

            builder.HasMany(b => b.TransactionProperties).WithMany(p => p.CriteriaToMatchOn);
            builder.Navigation(b => b.TransactionProperties).AutoInclude();

            builder.HasMany(b => b.TransactionCategories).WithMany(c => c.CriteriaToMatchOn);
            builder.Navigation(b => b.TransactionCategories).AutoInclude();

        }
    }
}
