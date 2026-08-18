
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

            //services.AddScoped<IConfiguredMovementRepository, ConfiguredMovementRepository>();
            //services.AddScoped<IMovementRepository, MovementRepository>();
            //services.AddScoped<IMovementConfigurationRepository, MovementConfigurationRepository>();
            //services.AddScoped<IWorkoutTemplateRepository, WorkoutTemplateRepository>();

            //services.AddScoped<IWorkoutInstanceRepository, WorkoutInstanceRepository>();
            //services.AddScoped<IWorkoutInstanceMovementRepository, WorkoutInstanceMovementRepository>();
            //services.AddScoped<IWorkoutInstanceMovementSetRepository, WorkoutInstanceMovementSetRepository>();
            return services;
        }
    }
}
