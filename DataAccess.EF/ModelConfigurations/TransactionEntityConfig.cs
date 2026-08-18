using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.EF.ModelConfigurations
{
    public class TransactionEntityConfig : IEntityTypeConfiguration<object>
    {
        public void Configure(EntityTypeBuilder<object> builder)
        {
            //builder.ToTable("ConfiguredMovements");
            //builder.HasKey(x => x.Id);
        }
    }
}
