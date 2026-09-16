using DataAccess.EF.ModelConfigurations.Base;
using DataAccess.Models.ChangeRequestEntities.Line;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.EF.ModelConfigurations.ChangeRequestEntities.Line
{
    internal class ChangeRequestCreateLineConfig : TransactionDataChangeRequestLineConfig, IEntityTypeConfiguration<ChangeRequestCreateLine>
    {
        new public void Configure(EntityTypeBuilder<ChangeRequestCreateLine> builder)
        {
            base.Configure(builder);

            builder.HasMany(b => b.TransactionCategories).WithMany();
            builder.HasMany(b => b.TransactionProperties).WithMany();
        }
    }
}
