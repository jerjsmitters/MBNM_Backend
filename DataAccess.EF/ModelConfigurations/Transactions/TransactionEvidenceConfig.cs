using DataAccess.EF.ModelConfigurations.Base;
using DataAccess.Models.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.EF.ModelConfigurations.Transactions
{
    public class TransactionEvidenceConfig : IEntityTypeConfiguration<TransactionEvidence>
    {
        public void Configure(EntityTypeBuilder<TransactionEvidence> builder)
        {
            EntityBaseConfig.Configure(builder);            
            
            builder.Property(b => b.FileLocation).IsRequired();

            builder.HasOne(b => b.Transaction).WithMany(t => t.Evidence);
        }
    }
}
