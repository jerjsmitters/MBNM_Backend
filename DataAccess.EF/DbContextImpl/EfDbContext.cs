using DataAccess.Models;
using DataAccess.Models.ChangeRequest;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.EF.DbContextImpl
{
    public class EfDbContext(DbContextOptions<EfDbContext> dbContextOptions) : DbContext(dbContextOptions)
    {
        public DbSet<ChangeRequest> ChangeRequests { get; set; }
       // public DbSet<ChangeRequestEntryDelete> ChangeRequestTransactions { get; set; } TBI
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<TransactionCategory> TransactionCategories { get; set; }
        public DbSet<TransactionEvidence> TransactionEvidence { get; set; }
        public DbSet<TransactionListVersion> TransactionListVersions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Apply all IEntityTypeConfiguration implementations in this assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(EfDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }

    }
}
