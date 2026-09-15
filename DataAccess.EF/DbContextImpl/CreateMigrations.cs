using Common;
using Common.DbSetup;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.EF.DbContextImpl
{
    /// <summary>
    /// This is used purely to create migrations at design time. It is not used at runtime.
    /// </summary>
    public class CreateMigrations : IDesignTimeDbContextFactory<EfDbContext>
    {
        public EfDbContext CreateDbContext(string[] args)
        {
            var connString = ConnectionString.GetConnectionString();

            var options = new DbContextOptionsBuilder<EfDbContext>()
                .UseMySQL(connString)
                .Options;

            return new EfDbContext(options);
        }
    }
}
