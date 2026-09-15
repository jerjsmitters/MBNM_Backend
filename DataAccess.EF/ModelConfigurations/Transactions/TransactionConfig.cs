using DataAccess.EF.ModelConfigurations.Base;
using DataAccess.Models.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.EF.ModelConfigurations.Transactions
{
    internal class TransactionConfig : IEntityTypeConfiguration<Transaction>
    {
        new public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            EntityBaseConfig.Configure(builder);

            builder.Property(b => b.Amount).IsRequired();
            builder.Property(b => b.Date).IsRequired();
            builder.Property(b => b.Sequence).IsRequired();
            builder.Property(b => b.Reference).IsRequired(false);

            // Many-to-many to categories and properties (no inverse navigation on category/property for transactions)
            builder.HasMany(b => b.AssignnedCategories).WithMany();
            builder.HasMany(b => b.AssignedProperties).WithMany();

            // Evidence one-to-many
            builder.HasMany(b => b.Evidence).WithOne(e => e.Transaction);
        }
    }
}
