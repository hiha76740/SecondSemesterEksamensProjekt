using BookRight.ApplicationLib.Handlers.Bookings;
using BookRight.ApplicationLib.Services;
using BookRight.FacadeLib.Commands.Booking.Interfaces;
using BookRight.FacadeLib.Commands.Pricing.Interfaces;

namespace BookRight.Web.DependencyInjections
{
    public static class HandlerDI
    {
        public static IServiceCollection AddHandlerDI(this IServiceCollection services)
        {
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

            services.AddScoped<IPriceCalculatorService, PriceCalculatorService>();

            return services;
        }
    }
}