using BookRight.DomainLib.Enums;
using BookRight.DomainLib.Exceptions;
using BookRight.DomainLib.ValueObjects;

namespace BookRight.DomainLib.Entities
{
    public class Booking : AggregateRoot
    {
        public TimeSlot TimeSlot { get; private set; } = null!;

        // Andre Aggregate Roots refereres via ID - ikke via objektreferencer. 
        public Guid CustomerId { get; private set; }
        public Guid TreatmentId { get; private set; }
        public Guid TherapistId { get; private set; }
        public Guid ClinicId { get; private set; }

        // Bookingens egne oplysninger og tilstand.
        public bool IsActive { get; private set; }
        public decimal Price { get; private set; }
        public BookingStatus Status { get; private set; }

        // Parameterløs constructor til EF Core
        private Booking() { }

        // Privat constructor - tvinger brug af factory-metoden Opret()
        private Booking(
            Guid customerId,
            Guid treatmentId,
            Guid therapistId,
            Guid clinicId,
            TimeSlot timeSlot,
            decimal price)
        {
            if (price < 0)
            {
                throw new DomainException("Prisen må ikke være negativ.");
            }

            Id = Guid.NewGuid();

            CustomerId = customerId != Guid.Empty
                ? customerId : throw new DomainException("KundeId må ikke være tomt.");

            TreatmentId = treatmentId != Guid.Empty
                ? treatmentId : throw new DomainException("BehandlingsId må ikke være tomt.");

            TherapistId = therapistId != Guid.Empty
                ? therapistId : throw new DomainException("BehandlerId må ikke være tomt.");

            ClinicId = clinicId != Guid.Empty
                ? clinicId : throw new DomainException("KlinikId må ikke være tomt.");

            TimeSlot = timeSlot
                ?? throw new DomainException("Tidsinterval er påkrævet.");

            Price = price;
            IsActive = true;
            Status = BookingStatus.Created;
        }

        // Factory-metode til oprettelse af booking
        public static Booking Opret(
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
                customerId,
                treatmentId,
                therapistId,
                clinicId,
                timeSlot,
                price);

            booking.ValidateNoOverlap(
                timeSlot,
                therapistId,
                clinicId,
                existingTherapistBookings,
                existingClinicBookings);

            return booking;
        }

        // Opdateringslogik
        public void ChangeTime(
            TimeSlot newTimeSlot,
            IEnumerable<Booking> therapistBookings,
            IEnumerable<Booking> clinicBookings)
        {
            EnsureNotCompleted();

            newTimeSlot = newTimeSlot
                ?? throw new DomainException("Tidsinterval er påkrævet.");

            ValidateNoOverlap(
                newTimeSlot,
                TherapistId,
                ClinicId,
                therapistBookings,
                clinicBookings);

            TimeSlot = newTimeSlot;
        }

        public void ChangeTreatment(Guid treatmentId)
        {
            EnsureNotCompleted();

            TreatmentId = treatmentId != Guid.Empty
                ? treatmentId : throw new DomainException("BehandlingsId må ikke være tomt.");
        }

        public void ChangeTherapist(
            Guid therapistId,
            IEnumerable<Booking> therapistBookings)
        {
            EnsureNotCompleted();

            therapistId = therapistId != Guid.Empty
                ? therapistId : throw new DomainException("BehandlerId må ikke være tomt.");

            ValidateNoOverlap(
                TimeSlot,
                therapistId,
                ClinicId,
                therapistBookings,
                Enumerable.Empty<Booking>());

            TherapistId = therapistId;
        }

        public void ChangeClinic(
            Guid clinicId,
            IEnumerable<Booking> clinicBookings)
        {
            EnsureNotCompleted();

            clinicId = clinicId != Guid.Empty
                ? clinicId : throw new DomainException("KlinikId må ikke være tomt.");

            ValidateNoOverlap(
                TimeSlot,
                TherapistId,
                clinicId,
                Enumerable.Empty<Booking>(),
                clinicBookings);

            ClinicId = clinicId;
        }

        // Tilstandsændringer
        public void Cancel()
        {
            EnsureNotCompleted();

            Status = BookingStatus.Cancelled;
            IsActive = false;
        }

        public void Complete()
        {
            if (Status == BookingStatus.Cancelled)
            {
                throw new DomainException("En aflyst booking kan ikke afsluttes.");
            }

            if (Status == BookingStatus.NoShow)
            {
                throw new DomainException("En no-show booking kan ikke afsluttes.");
            }

            Status = BookingStatus.Completed;
            IsActive = false;
        }

        public void NoShow()
        {
            EnsureNotCompleted();

            if (Status == BookingStatus.Cancelled)
            {
                throw new DomainException("En aflyst booking kan ikke markeres som no-show.");
            }

            Status = BookingStatus.NoShow;
            IsActive = false;
        }

        public void Arrived()
        {
            EnsureNotCompleted();

            if (Status == BookingStatus.Cancelled)
            {
                throw new DomainException("En aflyst booking kan ikke markeres som ankommet.");
            }

            Status = BookingStatus.Arrived;
        }

        // Privat status-validering
        private void EnsureNotCompleted()
        {
            if (Status == BookingStatus.Completed)
            {
                throw new DomainException("En afsluttet booking kan ikke ændres.");
            }
        }

        // Privat overlap-validering
        private void ValidateNoOverlap(
            TimeSlot timeSlot,
            Guid therapistId,
            Guid clinicId,
            IEnumerable<Booking> therapistBookings,
            IEnumerable<Booking> clinicBookings)
        {
            if (therapistBookings != null)
            {
                var therapistOverlap = therapistBookings
                    .Where(booking => booking.Id != Id)
                    .Where(booking => booking.IsActive)
                    .Where(booking => booking.TherapistId == therapistId)
                    .FirstOrDefault(booking => timeSlot.OverlapsWith(booking.TimeSlot));

                if (therapistOverlap is not null)
                {
                    throw new DomainException(
                        $"Behandleren har allerede en booking " +
                        $"({therapistOverlap.TimeSlot.From:HH:mm}-{therapistOverlap.TimeSlot.To:HH:mm}) " +
                        $"der overlapper med det ønskede tidsinterval.");
                }
            }

            if (clinicBookings != null)
            {
                var clinicOverlap = clinicBookings
                    .Where(booking => booking.Id != Id)
                    .Where(booking => booking.IsActive)
                    .Where(booking => booking.ClinicId == clinicId)
                    .FirstOrDefault(booking => timeSlot.OverlapsWith(booking.TimeSlot));

                if (clinicOverlap is not null)
                {
                    throw new DomainException(
                        $"Klinikken har allerede en booking " +
                        $"({clinicOverlap.TimeSlot.From:HH:mm}-{clinicOverlap.TimeSlot.To:HH:mm}) " +
                        $"der overlapper med det ønskede tidsinterval.");
                }
            }
        }
    }
}
