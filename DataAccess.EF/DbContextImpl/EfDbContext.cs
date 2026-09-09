using Common.Dto.ChangeRequest;
using DataAccess.Models.ChangeRequestEntities;
using DataAccess.Models.ChangeRequestEntities.Line;
using DataAccess.Models.Transactions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
        public DbSet<TransactionEvidence> TransactionEvidence { get; set; }
        public DbSet<MatchingCriteria> MatchingCriteria { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Apply all IEntityTypeConfiguration implementations in this assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(EfDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }

    }
}
