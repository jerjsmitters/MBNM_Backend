using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.EF.DbContextImpl
{
    public class EfDbContext(DbContextOptions<EfDbContext> dbContextOptions) : DbContext(dbContextOptions)
    {
        //public DbSet<WorkoutInstance> WorkoutInstances { get; set; }
        //public DbSet<WorkoutInstanceMovement> WorkoutInstanceMovements { get; set; }
        //public DbSet<WorkoutInstanceMovementSet> WorkoutInstanceMovementSets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Apply all IEntityTypeConfiguration implementations in this assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(EfDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }

    }
}
