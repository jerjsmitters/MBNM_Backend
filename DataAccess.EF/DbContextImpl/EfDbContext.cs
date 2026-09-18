using Common.Dto.ChangeRequest;
using DataAccess.Models.Base;
using DataAccess.Models.ChangeRequestEntities;
using DataAccess.Models.ChangeRequestEntities.Line;
using DataAccess.Models.Criteria;
using DataAccess.Models.Transactions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace DataAccess.EF.DbContextImpl
{
    public class EfDbContext(DbContextOptions<EfDbContext> dbContextOptions) : DbContext(dbContextOptions)
    {
        public DbSet<ChangeRequest> ChangeRequests { get; set; }
        public DbSet<ChangeRequestCreateLine> ChangeRequestCreateLines { get; set; }
        public DbSet<ChangeRequestUpdateLine> ChangeRequestUpdateLines { get; set; }
        public DbSet<ChangeRequestDeleteLine> ChangeRequestDeleteLines { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<TransactionCategory> TransactionCategories { get; set; }
        public DbSet<TransactionProperty> TransactionProperties { get; set; }
        public DbSet<TransactionEvidence> TransactionEvidence { get; set; }
        public DbSet<MatchingCriteria> MatchingCriteria { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries()
                      .Where(x => x.Entity is EntityBase)
                      .Where(x => x.State == EntityState.Added || x.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                var entity = entry.Entity as EntityBase;

                if (entry.State == EntityState.Added)
                {
                    entity.CreatedOn = DateTime.UtcNow;
                }
                else
                {
                    entity.UpdatedOn = DateTime.UtcNow;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Apply all IEntityTypeConfiguration implementations in this assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(EfDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }

    }
}
