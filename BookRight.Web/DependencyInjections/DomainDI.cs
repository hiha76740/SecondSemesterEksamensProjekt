using BookRight.DomainLib.Services;

namespace BookRight.Web.DependencyInjections
{
    public static class DomainDI
    {
        public static IServiceCollection AddDomainDI(this IServiceCollection services)
        {
            services.AddScoped<IBookingCapacityService, BookingCapacityService>();
            services.AddScoped<IValidateOverlapService, ValidateOverlapService>();

            return services;
        }
    }
}
