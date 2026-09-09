using DataAccess.EF.ModelConfigurations.Base;
using DataAccess.Models.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.EF.ModelConfigurations.Transactions
{
    internal class TransactionConfig : EntityTypeConfigurationBase<Transaction>
    {
        new public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder
                .Property(b => b.Amount)
                .IsRequired();

            builder
                .Property(b => b.Date)
                .IsRequired();

            builder
                .Property(b => b.Sequence)
                .IsRequired();

            builder
                .Property(b => b.Reference)
                .IsRequired();

            builder
                .Property(b => b.Deleted)
                .HasDefaultValue(false)
                .IsRequired();

            // FKs
            builder.HasMany(b => b.Properties);

            builder.HasMany(b => b.Categories);

            builder
                .HasMany(b => b.Evidence)
                .WithOne(b => b.Transaction);


            base.Configure(builder);
        }
    }
}
