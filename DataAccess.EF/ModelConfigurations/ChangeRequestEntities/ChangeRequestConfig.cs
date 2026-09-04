using DataAccess.EF.ModelConfigurations.Base;
using DataAccess.Models.ChangeRequestEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.EF.ModelConfigurations.ChangeRequestEntities
{
    internal class ChangeRequestConfig : EntityTypeConfigurationBase<ChangeRequest>
    {
        new public void Configure(EntityTypeBuilder<ChangeRequest> builder)
        {
            builder
                .HasMany(b => b.CreateLines)
                .WithOne(b => b.ChangeRequest);
            builder
                .HasMany(b => b.UpdateLines)
                .WithOne(b => b.ChangeRequest); ;
            builder
                .HasMany(b => b.DeleteLines)
                .WithOne(b => b.ChangeRequest);

            base.Configure(builder);
        }
    }
}
