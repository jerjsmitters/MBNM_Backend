using DataAccess.Models.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.EF.ModelConfigurations.Transactions
{
    public class TransactionCategoryConfig : IEntityTypeConfiguration<TransactionCategory>
    {
        public void Configure(EntityTypeBuilder<TransactionCategory> builder)
        {
            //builder.ToTable("ConfiguredMovements");
            //builder.HasKey(x => x.Id);

            //inherited
            builder.HasKey(b => b.Id);

            builder
                .Property(b => b.CreatedOn)
                .IsRequired();
        }
    }
}
