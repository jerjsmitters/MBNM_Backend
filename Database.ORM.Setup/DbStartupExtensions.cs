using DataAccess.EF.Setup;
using Microsoft.Extensions.DependencyInjection;

namespace Database.ORM.Setup
{
    public static class DbStartupExtensions
    {
        public static IServiceCollection InitialiseORM(this IServiceCollection services, string connString)
        {
            //you can easily swap out the ORM used here
            services.InitialiseEF(connString);

            return services;
        }
    }
}
