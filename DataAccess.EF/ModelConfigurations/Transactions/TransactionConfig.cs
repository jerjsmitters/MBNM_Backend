using DataAccess.EF.ModelConfigurations.Base;
using DataAccess.Models.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DataAccess.EF.ModelConfigurations.Transactions
{
    internal class TransactionConfig : EntityBaseConfig, IEntityTypeConfiguration<Transaction>
    {
        new public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            base.Configure(builder);

            //required as oracles mysql package is v broken and Pomelo is yet to be ported to dotnet 10
            var dateConverter = new ValueConverter<DateOnly, DateTime>(
                d => d.ToDateTime(TimeOnly.MinValue),
                dt => DateOnly.FromDateTime(dt));

            builder
                .Property(x => x.Date)
                .HasConversion(dateConverter)
                .IsRequired();

            builder.Property(b => b.Amount).IsRequired();
            builder.Property(b => b.Date).IsRequired();
            builder.Property(b => b.Sequence).IsRequired();
            builder.Property(b => b.Reference).IsRequired(false);

            // Many-to-many to categories and properties (no inverse navigation on category/property for transactions)
            builder.HasMany(b => b.AssignedCategories).WithMany();
            builder.HasMany(b => b.AssignedProperties).WithMany();

            // Evidence one-to-many
            builder.HasMany(b => b.Evidence).WithOne(e => e.Transaction);
        }
    }
}
