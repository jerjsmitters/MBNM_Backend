using DataAccess.EF.ModelConfigurations.Base;
using DataAccess.Models.ChangeRequestEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.EF.ModelConfigurations.ChangeRequestEntities
{
    internal class ChangeRequestConfig : EntityBaseConfig, IEntityTypeConfiguration<ChangeRequest>
    {
        new public void Configure(EntityTypeBuilder<ChangeRequest> builder)
        {
            base.Configure(builder);

            builder.HasMany(b => b.CreateLines).WithOne(l => l.ChangeRequest);
            builder.Navigation(b => b.CreateLines).AutoInclude();

            builder.HasMany(b => b.DeleteLines).WithOne(l => l.ChangeRequest);
            builder.Navigation(b => b.DeleteLines).AutoInclude();
        }
    }
}
