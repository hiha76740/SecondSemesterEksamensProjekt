using BookRight.ApplicationLib.Handlers.Bookings;
using BookRight.ApplicationLib.Handlers.Campaigns;
using BookRight.ApplicationLib.Handlers.Clinics;
using BookRight.ApplicationLib.Handlers.Customers;
using BookRight.ApplicationLib.Handlers.Therapists;
using BookRight.ApplicationLib.Services;
using BookRight.FacadeLib.Commands.Booking.Interfaces;
using BookRight.FacadeLib.Commands.Campaigns.Interfaces;
using BookRight.FacadeLib.Commands.Clinics.Interfaces;
using BookRight.FacadeLib.Commands.Customers.Interfaces;
using BookRight.FacadeLib.Commands.Pricing.Interfaces;
using BookRight.FacadeLib.Commands.Therapists.Interfaces;

namespace BookRight.Web.DependencyInjections
{
    public static class HandlerDI
    {
        public static IServiceCollection AddHandlerDI(this IServiceCollection services)
        {
            // Booking
            services.AddScoped<ICreateBookingHandler, CreateBookingHandler>();
            services.AddScoped<ICancelBookingHandler, CancelBookingHandler>();
            services.AddScoped<IChangeClinicHandler, ChangeClinicHandler>();
            services.AddScoped<IChangeTherapistHandler, ChangeTherapistHandler>();
            services.AddScoped<IChangeTimeHandler, ChangeTimeHandler>();
            services.AddScoped<IChangeTreatmentHandler, ChangeTreatmentHandler>();
            services.AddScoped<ICompleteBookingHandler, CompleteBookingHandler>();
            services.AddScoped<ICustomerArrivedHandler, CustomerArrivedHandler>();
            services.AddScoped<IRegisterNoShowHandler, RegisterNoShowHandler>();
            services.AddScoped<IAddParticipantHandler, AddParticipantHandler>();
            services.AddScoped<IRemoveParticipantHandler, RemoveParticipantHandler>();

            // Price Calculator
            services.AddScoped<IPriceCalculatorService, PriceCalculatorService>();

            // Customer
            services.AddScoped<ICreateCustomerHandler, CreateCustomerHandler>();
            services.AddScoped<IChangeCustomerInfoHandler, ChangeCustomerInfoHandler>();

            // Therapist
            services.AddScoped<ICreateTherapistHandler, CreateTherapistHandler>();
            services.AddScoped<IChangeTherapistInfoHandler, ChangeTherapistInfoHandler>();

            // Clinic
            services.AddScoped<ICreateClinicHandler, CreateClinicHandler>();
            services.AddScoped<IChangeClinicInfoHandler, ChangeClinicInfoHandler>();

            // Campaign
            services.AddScoped<ICreateCampaignHandler, CreateCampaignHandler>();

            return services;
        }
    }
}