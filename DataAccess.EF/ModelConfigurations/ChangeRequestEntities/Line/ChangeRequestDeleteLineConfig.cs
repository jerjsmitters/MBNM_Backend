using DataAccess.EF.ModelConfigurations.Base;
using DataAccess.Models.ChangeRequestEntities.Line;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.EF.ModelConfigurations.ChangeRequestEntities.Line
{
    internal class ChangeRequestDeleteLineConfig : IEntityTypeConfiguration<ChangeRequestDeleteLine>
    {
        new public void Configure(EntityTypeBuilder<ChangeRequestDeleteLine> builder)
        {
            ChangeRequestLineConfig.Configure(builder);

            // Delete line references the original Transaction
            builder.HasOne(b => b.Transaction).WithMany();
        }
    }
}
