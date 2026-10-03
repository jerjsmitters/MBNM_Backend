using DataAccess.EF.ModelConfigurations.Base;
using DataAccess.Models.ChangeRequestEntities.Line;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.EF.ModelConfigurations.ChangeRequestEntities.Line
{
    internal class ChangeRequestDeleteLineConfig : ChangeRequestLineConfig, IEntityTypeConfiguration<ChangeRequestDeleteLine>
    {
        new public void Configure(EntityTypeBuilder<ChangeRequestDeleteLine> builder)
        {
            base.Configure(builder);

            // Delete line references the original Transaction
            builder.HasOne(b => b.Transaction).WithMany();
           //builder.Navigation(b => b.Transaction).AutoInclude();

        }
    }
}
