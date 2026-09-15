using DataAccess.EF.ModelConfigurations.Base;
using DataAccess.Models.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.EF.ModelConfigurations.Transactions
{
    public class TransactionPropertyConfig : IEntityTypeConfiguration<TransactionProperty>
    {
        public void Configure(EntityTypeBuilder<TransactionProperty> builder)
        {
            EntityBaseConfig.Configure(builder);
            builder.Property(b => b.Description).IsRequired();

            // Many-to-many with MatchingCriteria
            builder.HasMany(b => b.CriteriaToMatchOn).WithMany(c => c.TransactionProperties);
        }
    }
}
