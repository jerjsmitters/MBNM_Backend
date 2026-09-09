using DataAccess.EF.ModelConfigurations.Base;
using DataAccess.Models.ChangeRequestEntities.Line;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.EF.ModelConfigurations.ChangeRequestEntities.Line
{
    internal class ChangeRequestUpdateLineConfig : EntityTypeConfigurationBase<ChangeRequestUpdateLine>
    {
        new public void Configure(EntityTypeBuilder<ChangeRequestUpdateLine> builder)
        {
            builder.HasMany(x => x.TransactionProperties);
            builder.HasMany(x => x.TransactionCategories);
            builder.HasOne(x => x.Transaction);
            base.Configure(builder);
        }
    }
}
