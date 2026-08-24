using DataAccess.EF.DbContextImpl;
using DataAccess.EF.Setup;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Setup
{
    public static class ServiceSetupExtensions
    {
        public static IServiceCollection SetupServices(this IServiceCollection services, string connString)
        {
            services.InitialiseEF(connString);

            return services;
        }
    }
}
