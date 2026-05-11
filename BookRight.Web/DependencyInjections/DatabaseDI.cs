using BookRight.InfrastructureLib.Persistance.DbContextFiles;
using Microsoft.EntityFrameworkCore;

namespace BookRight.Web.DependencyInjections
{
    public static class DatabaseDI
    {
        public static IServiceCollection AddDatabaseDI(this IServiceCollection services, IConfiguration configuration)
        {
            var conn = configuration.GetConnectionString("Default");
            services.AddDbContext<BookRightDbContext>(options => 
            options.UseSqlServer(conn));

            return services;
        }
    }
}
