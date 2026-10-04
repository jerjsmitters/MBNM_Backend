using DataAccess.EF.ModelConfigurations.Base;
using DataAccess.Models.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.EF.ModelConfigurations.Transactions
{
    public class TransactionEvidenceConfig : EntityBaseConfig, IEntityTypeConfiguration<TransactionEvidence>
    {
        public void Configure(EntityTypeBuilder<TransactionEvidence> builder)
        {
            base.Configure(builder);            
            
            builder.Property(b => b.FileLocation).IsRequired();

            builder.HasOne(b => b.Transaction).WithMany(t => t.Evidence).IsRequired();
            //builder.Navigation(b => b.Transaction).AutoInclude();

        }
    }
}
