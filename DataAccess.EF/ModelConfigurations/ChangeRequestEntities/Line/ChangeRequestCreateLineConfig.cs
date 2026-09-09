using DataAccess.EF.ModelConfigurations.Base;
using DataAccess.Models.ChangeRequestEntities.Line;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.EF.ModelConfigurations.ChangeRequestEntities.Line
{
    internal class ChangeRequestCreateLineConfig : EntityTypeConfigurationBase<ChangeRequestCreateLine>
    {
        new public void Configure(EntityTypeBuilder<ChangeRequestCreateLine> builder)
        {
            builder.HasMany(x => x.TransactionProperties);
            builder.HasMany(x => x.TransactionCategories);
            base.Configure(builder);
        }
    }
}
