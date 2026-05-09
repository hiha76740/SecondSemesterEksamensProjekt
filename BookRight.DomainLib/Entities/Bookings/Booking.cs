using BookRight.DomainLib.Enums;
using BookRight.DomainLib.Exceptions;
using BookRight.DomainLib.ValueObjects;

namespace BookRight.DomainLib.Entities.Bookings;

/// <summary>
/// Represents a scheduled treatment booking for a customer at a clinic, including details such as time slot, treatment,
/// therapist, and status.
/// </summary>
/// <remarks>A Booking is the aggregate root for managing the lifecycle of a treatment appointment, including
/// creation, modification, cancellation, and completion. Bookings enforce business rules such as preventing overlapping
/// appointments for the same customer, therapist, or clinic. All referenced entities are identified by their unique IDs
/// rather than object references. Thread safety is not guaranteed; concurrent modifications should be managed
/// externally.</remarks>
public class Booking : AggregateRoot
{
    // Other Aggregate Roots are referenced by ID - not object references.
    public Guid TreatmentId { get; private set; }
    public Guid TherapistId { get; private set; }
    public Guid ClinicId { get; private set; }

    public BookingStatus Status { get; private set; }
    public TimeSlot TimeSlot { get; private set; }
    public decimal Price { get; private set; }

    public int ParticipantLimit { get; private set; }
    public bool IsActive => Status != BookingStatus.Cancelled;


    private readonly List<Guid> _participants = new();
    public IReadOnlyList<Guid> Participants => _participants.AsReadOnly();


    /// <summary>
    /// Creates a new booking for the specified time slot, customer, treatment, therapist, and clinic, ensuring there
    /// are no overlapping bookings.
    /// </summary>
    /// <remarks>If the new booking overlaps with any existing booking for the customer or therapist, an
    /// exception is thrown. Ensure that the provided collections contain all relevant bookings to avoid
    /// conflicts.</remarks>
    /// <param name="timeSlot">The time slot for the booking. Specifies the start and end time of the appointment.</param>
    /// <param name="customerId">The unique identifier of the customer for whom the booking is created.</param>
    /// <param name="treatmentId">The unique identifier of the treatment to be performed during the booking.</param>
    /// <param name="therapistId">The unique identifier of the therapist assigned to the booking.</param>
    /// <param name="clinicId">The unique identifier of the clinic where the booking will take place.</param>
    /// <param name="price">The price to be charged for the booking. Must be a non-negative value.</param>
    /// <param name="existingCustomerBookings">A collection of the customer's existing bookings. Used to check for overlapping appointments.</param>
    /// <param name="existingTherapistBookings">A collection of the therapist's existing bookings. Used to check for overlapping appointments.</param>
    /// <returns>A new Booking instance representing the scheduled appointment.</returns>
    public static Booking Create(
        TimeSlot timeSlot,
        Guid treatmentId,
        Guid therapistId,
        Guid clinicId,
        decimal price,
        IEnumerable<Booking> existingCustomerBookings,
        IEnumerable<Booking> existingTherapistBookings,
        int participantLimit,
        Guid? customerId = null)
    {
        if (participantLimit < 1)
            throw new DomainException("A booking must allow at least one participant.");

        if (participantLimit == 1 && customerId == null)
            throw new DomainException("Single-person bookings require a customer.");


        var booking = new Booking(timeSlot, treatmentId, therapistId, clinicId, price, participantLimit);

        ValidateNoOverlap(booking, existingCustomerBookings, existingTherapistBookings);

        if (customerId != null)
            booking.AddParticipant(customerId.Value);

        return booking;
    }

    /// <summary>
    /// Changes the time slot for the current booking to the specified value, ensuring no scheduling conflicts for the
    /// customer or therapist.
    /// </summary>
    /// <remarks>If the new time slot overlaps with any existing booking for the customer or therapist, the
    /// change is reverted and an exception is thrown.</remarks>
    /// <param name="newTimeSlot">The new time slot to assign to the booking. Must not overlap with existing bookings for the customer or
    /// therapist.</param>
    /// <param name="existsForCustomer">A collection of existing bookings for the customer. Used to check for scheduling conflicts.</param>
    /// <param name="existsForTherapist">A collection of existing bookings for the therapist. Used to check for scheduling conflicts.</param>
    public void ChangeTime(TimeSlot newTimeSlot, IEnumerable<Booking> existsForCustomer, IEnumerable<Booking> existsForTherapist)
    {
        EnsureCanBeChanged();

        TimeSlot oldTimeSlot = TimeSlot;

        TimeSlot = newTimeSlot;

        try
        {
            ValidateNoOverlap(this, existsForCustomer, existsForTherapist);
        }
        catch (DomainException)
        {
            TimeSlot = oldTimeSlot;
            throw;
        }
    }

    /// <summary>
    /// Changes the current treatment to the specified treatment identifier.
    /// </summary>
    /// <param name="newTreatmentId">The unique identifier of the new treatment to assign. Must not be equal to the current treatment identifier.</param>
    /// <exception cref="DomainException">Thrown if the specified treatment identifier is the same as the current treatment identifier.</exception>
    public void ChangeTreatment(Guid newTreatmentId)
    {
        EnsureCanBeChanged();

        if (TreatmentId == newTreatmentId)
            throw new DomainException("new and old treatment can't be the same");

        TreatmentId = newTreatmentId;
    }

    /// <summary>
    /// Changes the assigned therapist for this booking to the specified therapist.
    /// </summary>
    /// <param name="newTherapistId">The unique identifier of the new therapist to assign to this booking. Must not be the same as the current
    /// therapist.</param>
    /// <param name="existsForNewTherapist">A collection of existing bookings for the new therapist. Used to ensure there are no scheduling conflicts with
    /// the new assignment.</param>
    /// <exception cref="DomainException">Thrown if the new therapist is the same as the current therapist, or if the change would result in overlapping
    /// bookings.</exception>
    public void ChangeTherapist(Guid newTherapistId, IEnumerable<Booking> existsForNewTherapist)
    {
        EnsureCanBeChanged();

        if (newTherapistId == TherapistId)
            throw new DomainException("New and old therapist can't be the same");

        ValidateNoOverlap(this, existsForNewTherapist);

        TherapistId = newTherapistId;
    }

    /// <summary>
    /// Changes the associated clinic to the specified clinic identifier.
    /// </summary>
    /// <param name="newClinicId">The unique identifier of the new clinic to associate with. Must not be equal to the current clinic identifier.</param>
    /// <exception cref="DomainException">Thrown if the specified clinic identifier is the same as the current clinic identifier.</exception>
    public void ChangeClinic(Guid newClinicId)
    {
        EnsureCanBeChanged();

        if (newClinicId == ClinicId)
            throw new DomainException("New and old clinic can't be the same");

        ClinicId = newClinicId;
    }

    /// <summary>
    /// Cancels the booking and updates its status to indicate it is no longer active.
    /// </summary>
    /// <remarks>After calling this method, the booking cannot be modified further. If the booking is already
    /// cancelled or cannot be changed, an exception may be thrown.</remarks>
    public void Cancel()
    {
        EnsureCanBeChanged();

        Status = BookingStatus.Cancelled;
    }

    /// <summary>
    /// Marks the booking as completed, updating its status accordingly.
    /// </summary>
    /// <remarks>This method can only be called when the booking is in a state that allows changes. An
    /// exception is thrown if the booking cannot be modified.</remarks>
    public void Complete()
    {
        EnsureCanBeChanged();

        Status = BookingStatus.Completed;
    }

    /// <summary>
    /// Marks the booking as a no-show, updating its status accordingly.
    /// </summary>
    /// <remarks>Call this method when a booked guest fails to appear. The booking status will be set to
    /// indicate a no-show. This operation may not be reversible depending on the booking workflow.</remarks>
    public void NoShow()
    {
        EnsureCanBeChanged();

        Status = BookingStatus.NoShow;
    }

    /// <summary>
    /// Marks the booking as having arrived by updating its status to Arrived.
    /// </summary>
    /// <remarks>Call this method when the associated entity has reached its destination or check-in point.
    /// This method may throw an exception if the booking cannot be changed in its current state.</remarks>
    public void Arrived()
    {
        EnsureCanBeChanged();

        Status = BookingStatus.Arrived;
    }

    public void AddParticipant(Guid customerId)
    {
        EnsureIsActive();

        if (_participants.Count == ParticipantLimit)
            throw new DomainException("Booking has reach limit for participants");

        if (_participants.Contains(customerId) == true)
            throw new DomainException("Customer is already added to this booking");

        _participants.Add(customerId);
    }

    public void RemoveParticipant(Guid customerId)
    {
        EnsureIsActive();

        if (_participants.Contains(customerId) == false)
            throw new DomainException("The customer you are trying to remove is not registeret to this booking");

        _participants.Remove(customerId);

        if (_participants.Count == 0)
        {
            Status = BookingStatus.Cancelled;
        }

    }

    /// <summary>
    /// Initializes a new instance of the Booking class with the specified time slot, customer, treatment, therapist,
    /// clinic, and price.
    /// </summary>
    /// <param name="timeSlot">The time slot for the booking.</param>
    /// <param name="customerId">The unique identifier of the customer making the booking.</param>
    /// <param name="treatmentId">The unique identifier of the treatment to be booked.</param>
    /// <param name="therapistId">The unique identifier of the therapist assigned to the booking.</param>
    /// <param name="clinicId">The unique identifier of the clinic where the booking takes place.</param>
    /// <param name="price">The price of the booking. Must be zero or positive.</param>
    /// <exception cref="DomainException">Thrown if price is negative.</exception>
    private Booking(TimeSlot timeSlot, Guid treatmentId, Guid therapistId, Guid clinicId, decimal price, int participantLimit)
    {
        if (price < 0)
            throw new DomainException("Price cannot be negative.");

        Id = Guid.NewGuid();
        TimeSlot = timeSlot;
        TreatmentId = treatmentId;
        TherapistId = therapistId;
        ClinicId = clinicId;
        Price = price;
        ParticipantLimit = participantLimit;
        Status = BookingStatus.Created;
    }


    private void EnsureIsActive()
    {
        if (IsActive == false)
            throw new DomainException("Booking is no longer active");
    }


    /// <summary>
    /// Ensures that the booking can be changed by validating its current status.
    /// </summary>
    /// <remarks>This method should be called before performing any operation that modifies the booking. It
    /// prevents changes to bookings that are no longer eligible for modification.</remarks>
    /// <exception cref="DomainException">Thrown if the booking status is either Cancelled or Completed.</exception>
    private void EnsureCanBeChanged()
    {
        if (Status == BookingStatus.Cancelled)
            throw new DomainException("Cancelled booking cannot be changed.");

        if (Status == BookingStatus.Completed)
            throw new DomainException("Completed booking cannot be changed.");
    }

    /// <summary>
    /// Validates that the specified booking does not overlap with any of the existing bookings.
    /// </summary>
    /// <param name="booking">The booking to validate for overlap. Cannot be null.</param>
    /// <param name="existingBookings">A collection of existing bookings to check against for overlap. Cannot be null.</param>
    private static void ValidateNoOverlap(Booking booking, IEnumerable<Booking> existingBookings)
    {
        ValidateNoOverlap(booking, existingBookings, Array.Empty<Booking>());
    }

    /// <summary>
    /// Validates that the specified booking does not overlap with any existing active bookings for the same customer or
    /// therapist.
    /// </summary>
    /// <param name="booking">The booking to validate for time slot overlap. Cannot be null.</param>
    /// <param name="existingCustomerBookings">A collection of the customer's existing bookings to check for overlapping time slots. Only active bookings are
    /// considered.</param>
    /// <param name="existingTherapistBookings">A collection of the therapist's existing bookings to check for overlapping time slots. Only active bookings are
    /// considered.</param>
    /// <exception cref="DomainException">Thrown if the booking's time slot overlaps with any active booking for the customer or therapist.</exception>
    private static void ValidateNoOverlap(Booking booking, IEnumerable<Booking> existingCustomerBookings, IEnumerable<Booking> existingTherapistBookings)
    {
        bool customerOverlap = existingCustomerBookings.Any(cb =>
            cb.Id != booking.Id &&
            cb.IsActive == true &&
            cb.TimeSlot.OverlapsWith(booking.TimeSlot));

        bool therapistOverlap = existingTherapistBookings.Any(tb =>
            tb.Id != booking.Id &&
            tb.IsActive == true &&
            tb.TimeSlot.OverlapsWith(booking.TimeSlot));

        if (customerOverlap == true || therapistOverlap == true)
            throw new DomainException("Overlap in time.");
    }

    // Parameterløs constructor til EF Core
    private Booking() { }
}