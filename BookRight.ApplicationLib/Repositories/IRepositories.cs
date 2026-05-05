using BookRight.DomainLib.Entities.Bookings;
using BookRight.DomainLib.Entities.Clinics;
using BookRight.DomainLib.Entities.Customers;
using BookRight.DomainLib.Entities.Therapists;
using BookRight.DomainLib.Entities.Treatments;

namespace BookRight.ApplicationLib.Repositories;

// Repository-interfaces: kontrakt for datadgang til domæneobjekter.
// Placeres i Application-laget — implementeres i Infrastructure.


public interface IBookingRepository
{
    Task<Booking?> GetByIdAsync(Guid bookingId);

    Task<IReadOnlyList<Booking>> GetByCustomerIdAsync(Guid customerId);

    Task<IReadOnlyList<Booking>> GetByTherapistIdAsync(Guid therapistId);

    Task<IReadOnlyList<Booking>> GetByClinicIdAsync(Guid clinicId);

    Task AddAsync(Booking booking);

    Task SaveChangesAsync();
}

public interface ICustomerRepository
{
    Task<Customer?> GetByIdAsync(Guid customerId);
}

public interface ITherapistRepository
{
    Task<Therapist?> GetByIdAsync(Guid therapistId);
}

public interface IClinicRepository
{
    Task<Clinic?> GetByIdAsync(Guid clinicId);
}

public interface ITreatmentRepository
{
    Task<Treatment?> GetByIdAsync(Guid treatmentId);
}
