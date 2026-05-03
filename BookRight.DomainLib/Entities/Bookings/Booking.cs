using BookRight.DomainLib.Enums;
using BookRight.DomainLib.Exceptions;
using BookRight.DomainLib.ValueObjects;

namespace BookRight.DomainLib.Entities.Bookings
{
    /// <summary>
    /// Aggregate Root for Booking.
    /// Handles booking creation, updates, status changes and overlap validation.
    /// </summary>
    public class Booking : AggregateRoot
    {
        // Other Aggregate Roots are referenced by ID - not object references.
        public Guid CustomerId { get; init; }
        public Guid TreatmentId { get; private set; }
        public Guid TherapistId { get; private set; }
        public Guid ClinicId { get; private set; }

        public BookingStatus Status { get; private set; }
        public TimeSlot TimeSlot { get; private set; }
        public decimal Price { get; init; }

        public bool IsActive => Status != BookingStatus.Cancelled;

        /// <summary>
        /// Creates a new booking and validates that it does not overlap
        /// with existing bookings for the therapist or clinic.
        /// </summary>
        public static Booking Create(
            TimeSlot timeSlot,
            Guid customerId,
            Guid treatmentId,
            Guid therapistId,
            Guid clinicId,
            decimal price,
            IEnumerable<Booking> existingTherapistBookings,
            IEnumerable<Booking> existingClinicBookings)
        {
            var booking = new Booking(
                timeSlot,
                customerId,
                treatmentId,
                therapistId,
                clinicId,
                price);

            ValidateNoOverlap(
                booking,
                existingTherapistBookings,
                existingClinicBookings);

            return booking;
        }

        /// <summary>
        /// Changes the time of the booking and validates that the new time does not overlap.
        /// </summary>
        public void ChangeTime(
            TimeSlot newTimeSlot,
            IEnumerable<Booking> therapistBookings,
            IEnumerable<Booking> clinicBookings)
        {
            EnsureCanBeChanged();

            if (newTimeSlot == null)
                throw new DomainException("TimeSlot is required.");

            var bookingToValidate = CreateValidationBooking(
                newTimeSlot,
                TherapistId,
                ClinicId);

            ValidateNoOverlap(
                bookingToValidate,
                therapistBookings,
                clinicBookings);

            TimeSlot = newTimeSlot;
        }

        /// <summary>
        /// Changes the treatment connected to the booking.
        /// </summary>
        public void ChangeTreatment(Guid treatmentId)
        {
            EnsureCanBeChanged();

            if (treatmentId == Guid.Empty)
                throw new DomainException("Treatment Id cannot be empty.");

            TreatmentId = treatmentId;
        }

        /// <summary>
        /// Changes the therapist and validates that the booking does not overlap.
        /// </summary>
        public void ChangeTherapist(
            Guid therapistId,
            IEnumerable<Booking> therapistBookings)
        {
            EnsureCanBeChanged();

            if (therapistId == Guid.Empty)
                throw new DomainException("Therapist Id cannot be empty.");

            var bookingToValidate = CreateValidationBooking(
                TimeSlot,
                therapistId,
                ClinicId);

            ValidateNoOverlap(
                bookingToValidate,
                therapistBookings,
                true);

            TherapistId = therapistId;
        }

        /// <summary>
        /// Changes the clinic and validates that the booking does not overlap.
        /// </summary>
        public void ChangeClinic(
            Guid clinicId,
            IEnumerable<Booking> clinicBookings)
        {
            EnsureCanBeChanged();

            if (clinicId == Guid.Empty)
                throw new DomainException("Clinic Id cannot be empty.");

            var bookingToValidate = CreateValidationBooking(
                TimeSlot,
                TherapistId,
                clinicId);

            ValidateNoOverlap(
                bookingToValidate,
                clinicBookings,
                false);

            ClinicId = clinicId;
        }

        /// <summary>
        /// Cancels the booking.
        /// </summary>
        public void Cancel()
        {
            EnsureCanBeChanged();

            Status = BookingStatus.Cancelled;
        }

        /// <summary>
        /// Completes the booking.
        /// </summary>
        public void Complete()
        {
            EnsureCanBeChanged();

            Status = BookingStatus.Completed;
        }

        /// <summary>
        /// Marks the booking as no-show.
        /// </summary>
        public void NoShow()
        {
            EnsureCanBeChanged();

            Status = BookingStatus.NoShow;
        }

        /// <summary>
        /// Marks the customer as arrived.
        /// </summary>
        public void Arrived()
        {
            EnsureCanBeChanged();

            Status = BookingStatus.Arrived;
        }

        /// <summary>
        /// Private constructor used by the factory method to ensure valid creation.
        /// </summary>
        private Booking(
            TimeSlot timeSlot,
            Guid customerId,
            Guid treatmentId,
            Guid therapistId,
            Guid clinicId,
            decimal price)
        {
            if (timeSlot == null)
                throw new DomainException("TimeSlot is required.");

            if (customerId == Guid.Empty)
                throw new DomainException("Customer Id cannot be empty.");

            if (treatmentId == Guid.Empty)
                throw new DomainException("Treatment Id cannot be empty.");

            if (therapistId == Guid.Empty)
                throw new DomainException("Therapist Id cannot be empty.");

            if (clinicId == Guid.Empty)
                throw new DomainException("Clinic Id cannot be empty.");

            if (price < 0)
                throw new DomainException("Price cannot be negative.");

            Id = Guid.NewGuid();
            TimeSlot = timeSlot;
            CustomerId = customerId;
            TreatmentId = treatmentId;
            TherapistId = therapistId;
            ClinicId = clinicId;
            Price = price;
            Status = BookingStatus.Created;
        }

        /// <summary>
        /// Creates a temporary booking used only for validation.
        /// </summary>
        private Booking CreateValidationBooking(
            TimeSlot timeSlot,
            Guid therapistId,
            Guid clinicId)
        {
            var booking = new Booking(
                timeSlot,
                CustomerId,
                TreatmentId,
                therapistId,
                clinicId,
                Price);

            booking.Id = Id;

            return booking;
        }

        /// <summary>
        /// Ensures that cancelled or completed bookings cannot be changed.
        /// </summary>
        private void EnsureCanBeChanged()
        {
            if (Status == BookingStatus.Cancelled)
                throw new DomainException("Cancelled booking cannot be changed.");

            if (Status == BookingStatus.Completed)
                throw new DomainException("Completed booking cannot be changed.");
        }

        /// <summary>
        /// Validates that the booking does not overlap with existing active bookings
        /// for the same therapist or clinic.
        /// </summary>
        private static void ValidateNoOverlap(
            Booking booking,
            IEnumerable<Booking> existingTherapistBookings,
            IEnumerable<Booking> existingClinicBookings)
        {
            ValidateNoOverlap(
                booking,
                existingTherapistBookings,
                true);

            ValidateNoOverlap(
                booking,
                existingClinicBookings,
                false);
        }

        /// <summary>
        /// Validates that the booking does not overlap with existing active bookings
        /// for either the therapist or the clinic.
        /// </summary>
        private static void ValidateNoOverlap(
            Booking booking,
            IEnumerable<Booking> existingBookings,
            bool validateTherapist)
        {
            if (validateTherapist == true)
            {
                bool therapistOverlap = existingBookings.Any(existingBooking =>
                    existingBooking.Id != booking.Id &&
                    existingBooking.IsActive == true &&
                    existingBooking.TherapistId == booking.TherapistId &&
                    existingBooking.TimeSlot.OverlapsWith(booking.TimeSlot));

                if (therapistOverlap == true)
                    throw new DomainException("Therapist already has a booking in this time slot.");
            }

            if (validateTherapist == false)
            {
                bool clinicOverlap = existingBookings.Any(existingBooking =>
                    existingBooking.Id != booking.Id &&
                    existingBooking.IsActive == true &&
                    existingBooking.ClinicId == booking.ClinicId &&
                    existingBooking.TimeSlot.OverlapsWith(booking.TimeSlot));

                if (clinicOverlap == true)
                    throw new DomainException("Clinic already has a booking in this time slot.");
            }
        }

        // Parameterless constructor for EF Core
        private Booking() { }
    }
}