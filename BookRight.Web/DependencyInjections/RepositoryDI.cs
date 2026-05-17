using BookRight.ApplicationLib.Repositories;
using BookRight.InfrastructureLib.Repositories.Bookings;
using BookRight.InfrastructureLib.Repositories.Campaigns;
using BookRight.InfrastructureLib.Repositories.Clinics;
using BookRight.InfrastructureLib.Repositories.Customers;
using BookRight.InfrastructureLib.Repositories.Therapists;
using BookRight.InfrastructureLib.Repositories.Treatments;

namespace BookRight.Web.DependencyInjections;

public static class RepositoryDI
{
    public static IServiceCollection AddRepositoryDI(this IServiceCollection services)
    {
        services.AddScoped<IBookingRepository, BookingRepository>();
        services.AddScoped<ICampaignRepository, CampaignRepository>();
        services.AddScoped<IClinicRepository, ClinicRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<ITherapistRepository, TherapistRepository>();
        services.AddScoped<ITreatmentRepository, TreatmentRepository>();

        return services;
    }
}
