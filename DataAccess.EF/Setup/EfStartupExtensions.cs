
using DataAccess.EF.DbContextImpl;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess.EF.Setup
{
    public static class EfStartupExtensions
    {
        public static IServiceCollection InitialiseEF(this IServiceCollection services, string connString) 
        {
            services.AddDbContext<EfDbContext>(options =>
            {
                options.UseMySQL(connString);
            });

            return services;
        }
    }
}
