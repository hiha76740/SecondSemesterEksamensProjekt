using BookRight.FacadeLib.Queries.Bookings;
using BookRight.FacadeLib.Queries.Campaigns;
using BookRight.FacadeLib.Queries.CertificationTypes;
using BookRight.FacadeLib.Queries.Clinics;
using BookRight.FacadeLib.Queries.Customers;
using BookRight.FacadeLib.Queries.Therapists;
using BookRight.FacadeLib.Queries.Treatments;
using BookRight.InfrastructureLib.QueryHandlers.Bookings;
using BookRight.InfrastructureLib.QueryHandlers.Campaigns;
using BookRight.InfrastructureLib.QueryHandlers.CertificationType;
using BookRight.InfrastructureLib.QueryHandlers.Clinics;
using BookRight.InfrastructureLib.QueryHandlers.Customers;
using BookRight.InfrastructureLib.QueryHandlers.Therapists;
using BookRight.InfrastructureLib.QueryHandlers.Treatments;

namespace BookRight.Web.DependencyInjections;

public static class QueriesDI
{
    public static IServiceCollection AddQueriesDI(this IServiceCollection services)
    {
        services.AddScoped<IBookingQueries, BookingQueryHandlerIMPL>();
        services.AddScoped<ICampaignQueries, CampaignQueryHandlerIMPL>();
        services.AddScoped<IClinicQueries, ClinicQueryHandlerIMPL>();
        services.AddScoped<ICustomerQueries, CustomerQueryHandlerIMPL>();
        services.AddScoped<ITherapistQueries, TherapistQueryHandlerIMPL>();
        services.AddScoped<ITreatmentQueries, TreatmentQueryHandlerIMPL>();
        services.AddScoped<ICertificationTypeQueries, CertificationTypeQueryHandlerIMPL>();

        return services;
    }
}
