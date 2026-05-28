using BookRight.DomainLib.Enums;
using BookRight.DomainLib.Exceptions;
using BookRight.DomainLib.ValueObjects;

namespace BookRight.DomainLib.Entities.Bookings;

/// <summary>
/// Aggregate root representing a scheduled booking for a treatment at a clinic, including time slot, assigned
/// therapist, participants, price, discount, and lifecycle status.
/// </summary>
/// <remarks>Enforces domain invariants such as non-negative price, minimum participant limit, and that
/// single-person bookings require a customer. Prevents modifications when the booking status is Cancelled or Completed.
/// References other aggregates by identifier only and exposes operations to change time, assignment, manage
/// participants, and transition lifecycle state; domain rule violations result in DomainException.</remarks>
public class Booking : AggregateRoot
{
    public Guid TreatmentId { get; private set; }
    public Guid TherapistId { get; private set; }
    public Guid ClinicId { get; private set; }

    public BookingStatus Status { get; private set; }
    public TimeSlot Time { get; private set; } = null!;
    public decimal Price { get; private set; }

    public int ParticipantLimit { get; private set; }
    public bool IsActive => Status != BookingStatus.Cancelled;
    public DiscountTypes DiscountTypeUsed { get; init; }


    private readonly List<Guid> _participants = new();
    public IReadOnlyList<Guid> Participants => _participants.AsReadOnly();


   /// <summary>
   /// Creates a Booking with the specified time slot, treatment, therapist, clinic, price, participant limit, discount
   /// type, and optional customer.
   /// </summary>
   /// <remarks>If customerId is provided and participantLimit equals 1, the customer is automatically added as
   /// the booking's initial participant.</remarks>
   /// <param name="timeSlot">The scheduled time slot for the booking.</param>
   /// <param name="treatmentId">The identifier of the treatment.</param>
   /// <param name="therapistId">The identifier of the therapist.</param>
   /// <param name="clinicId">The identifier of the clinic.</param>
   /// <param name="price">The price charged for the booking.</param>
   /// <param name="participantLimit">The maximum number of participants; must be greater than or equal to 1.</param>
   /// <param name="discountTypeUsed">The discount type applied to the booking.</param>
   /// <param name="customerId">Optional identifier of the customer to add as the initial participant for single-person bookings.</param>
   /// <returns>The created Booking.</returns>
   /// <exception cref="DomainException">Thrown when participantLimit is less than 1, or when participantLimit equals 1 and customerId is null.</exception>
    public static Booking Create(
        TimeSlot timeSlot,
        Guid treatmentId,
        Guid therapistId,
        Guid clinicId,
        decimal price,
        int participantLimit,
        DiscountTypes discountTypeUsed,
        Guid? customerId = null)
    {
        if (participantLimit < 1)
            throw new DomainException("A booking must allow at least one participant.");

        if (participantLimit == 1 && customerId == null)
            throw new DomainException("Single-person bookings require a customer.");


        var booking = new Booking(timeSlot, treatmentId, therapistId, clinicId, price, participantLimit, discountTypeUsed);


        if (customerId != null && participantLimit == 1)
            booking.AddParticipant(customerId.Value);

        return booking;
    }

    /// <summary>
    /// Update the Time property to the specified TimeSlot after validating that the change is allowed.
    /// </summary>
    /// <remarks>Calls EnsureCanBeChanged before updating; an exception is thrown if the instance cannot be
    /// modified.</remarks>
    /// <param name="newTimeSlot">The TimeSlot to assign.</param>
    public void ChangeTime(TimeSlot newTimeSlot)
    {
        EnsureCanBeChanged();

        Time = newTimeSlot;
    }

    /// <summary>
    /// Changes the entity's treatment to the specified identifier.
    /// </summary>
    /// <param name="newTreatmentId">Identifier of the treatment to apply.</param>
    /// <exception cref="DomainException">Thrown if the specified newTreatmentId is equal to the current TreatmentId.</exception>
    public void ChangeTreatment(Guid newTreatmentId)
    {
        EnsureCanBeChanged();

        if (TreatmentId == newTreatmentId)
            throw new DomainException("new and old treatment can't be the same");

        TreatmentId = newTreatmentId;
    }

    /// <summary>
    /// Updates the entity's TherapistId to a new therapist after validating that the entity can be changed and that the
    /// new therapist differs from the current one.
    /// </summary>
    /// <param name="newTherapistId">The identifier of the therapist to assign.</param>
    /// <exception cref="DomainException">Thrown when the entity cannot be changed or when the supplied therapist identifier is the same as the current
    /// TherapistId.</exception>
    public void ChangeTherapist(Guid newTherapistId)
    {
        EnsureCanBeChanged();

        if (newTherapistId == TherapistId)
            throw new DomainException("New and old therapist can't be the same");

        TherapistId = newTherapistId;
    }

  /// <summary>
  /// Changes the associated clinic to the specified clinic identifier.
  /// </summary>
  /// <remarks>Validates that the entity can be changed before assigning the new ClinicId.</remarks>
  /// <param name="newClinicId">The identifier of the clinic to associate with the entity.</param>
  /// <exception cref="DomainException">Thrown when the specified clinic identifier equals the current clinic identifier.</exception>
    public void ChangeClinic(Guid newClinicId)
    {
        EnsureCanBeChanged();

        if (newClinicId == ClinicId)
            throw new DomainException("New and old clinic can't be the same");

        ClinicId = newClinicId;
    }

    /// <summary>
    /// Marks the booking as cancelled.
    /// </summary>
    /// <remarks>Validates that the booking can be changed by calling EnsureCanBeChanged, then sets Status to
    /// BookingStatus.Cancelled. Throws InvalidOperationException if the booking cannot be changed.</remarks>
    public void Cancel()
    {
        EnsureCanBeChanged();

        Status = BookingStatus.Cancelled;
    }

  /// <summary>
  /// Marks the booking as completed after validating that it can be changed.
  /// </summary>
  /// <remarks>Calls EnsureCanBeChanged, then sets the Status property to BookingStatus.Completed. Throws
  /// InvalidOperationException if the booking cannot be changed.</remarks>
    public void Complete()
    {
        EnsureCanBeChanged();

        Status = BookingStatus.Completed;
    }

    /// <summary>
    /// Marks the booking as a no-show.
    /// </summary>
    /// <remarks>Calls EnsureCanBeChanged to validate the booking can be modified before setting Status to
    /// BookingStatus.NoShow. May throw if the booking cannot be changed.</remarks>
    public void NoShow()
    {
        EnsureCanBeChanged();

        Status = BookingStatus.NoShow;
    }

  /// <summary>
  /// Marks the booking as arrived.
  /// </summary>
  /// <remarks>Validates that the booking can be changed before setting the Status property to
  /// BookingStatus.Arrived; EnsureCanBeChanged is invoked and may throw if modification is not allowed.</remarks>
    public void Arrived()
    {
        EnsureCanBeChanged();

        Status = BookingStatus.Arrived;
    }

    /// <summary>
    /// Adds the specified customer to the booking's participants.
    /// </summary>
    /// <remarks>Requires the booking to be active.</remarks>
    /// <param name="customerId">The identifier of the customer to add.</param>
    /// <exception cref="DomainException">The booking has reached its participant limit, or the customer is already a participant.</exception>
    public void AddParticipant(Guid customerId)
    {
        EnsureIsActive();

        if (_participants.Count == ParticipantLimit)
            throw new DomainException("Booking has reach limit for participants");

        if (_participants.Contains(customerId) == true)
            throw new DomainException("Customer is already added to this booking");

        _participants.Add(customerId);
    }

    /// <summary>
    /// Removes a participant from the booking.
    /// </summary>
    /// <remarks>If the last participant is removed and ParticipantLimit equals 1, the booking's status is set
    /// to Cancelled.</remarks>
    /// <param name="customerId">The identifier of the customer to remove.</param>
    /// <exception cref="DomainException">Thrown when the specified customer is not registered to this booking.</exception>
    public void RemoveParticipant(Guid customerId)
    {
        EnsureIsActive();

        if (_participants.Contains(customerId) == false)
            throw new DomainException("The customer you are trying to remove is not registeret to this booking");

        _participants.Remove(customerId);

        if (_participants.Count == 0 && ParticipantLimit == 1)
            Status = BookingStatus.Cancelled;
    }

   /// <summary>
   /// Initializes a new Booking with the specified time slot, treatment, therapist, clinic, price, participant limit,
   /// and discount; generates a new Id and sets Status to Created.
   /// </summary>
   /// <param name="timeSlot">Time slot for the booking.</param>
   /// <param name="treatmentId">Identifier of the treatment associated with the booking.</param>
   /// <param name="therapistId">Identifier of the therapist assigned to the booking.</param>
   /// <param name="clinicId">Identifier of the clinic where the booking takes place.</param>
   /// <param name="price">Price charged for the booking; must be non-negative.</param>
   /// <param name="participantLimit">Maximum number of participants allowed for the booking.</param>
   /// <param name="discountTypeUsed">Type of discount applied to the booking.</param>
   /// <exception cref="DomainException">Thrown when the provided price is negative.</exception>
    private Booking(TimeSlot timeSlot, Guid treatmentId, Guid therapistId, Guid clinicId, decimal price, int participantLimit, DiscountTypes discountTypeUsed)
    {
        if (price < 0)
            throw new DomainException("Price cannot be negative.");

        Id = Guid.NewGuid();
        Time = timeSlot;
        TreatmentId = treatmentId;
        TherapistId = therapistId;
        ClinicId = clinicId;
        Price = price;
        ParticipantLimit = participantLimit;
        DiscountTypeUsed = discountTypeUsed;
        Status = BookingStatus.Created;
    }

    /// <summary>
    /// Ensures the booking is active.
    /// </summary>
    /// <remarks>Serves as a guard before performing operations that require an active booking.</remarks>
    /// <exception cref="DomainException">Thrown when the booking is no longer active.</exception>
    private void EnsureIsActive()
    {
        if (IsActive == false)
            throw new DomainException("Booking is no longer active");
    }


   /// <summary>
   /// Ensures the booking can be changed.
   /// </summary>
   /// <exception cref="DomainException">Thrown when Status is BookingStatus.Cancelled or BookingStatus.Completed.</exception>
    private void EnsureCanBeChanged()
    {
        if (Status == BookingStatus.Cancelled)
            throw new DomainException("Cancelled booking cannot be changed.");

        if (Status == BookingStatus.Completed)
            throw new DomainException("Completed booking cannot be changed.");
    }

    private Booking() { }
}