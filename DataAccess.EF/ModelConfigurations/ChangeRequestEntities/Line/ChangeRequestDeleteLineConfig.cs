using DataAccess.EF.ModelConfigurations.Base;
using DataAccess.Models.ChangeRequestEntities.Line;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.EF.ModelConfigurations.ChangeRequestEntities.Line
{
    internal class ChangeRequestDeleteLineConfig : EntityTypeConfigurationBase<ChangeRequestDeleteLine>
    {
        new public void Configure(EntityTypeBuilder<ChangeRequestDeleteLine> builder)
        {
            builder.HasOne(x => x.Transaction);
            base.Configure(builder);
        }
    }
}
