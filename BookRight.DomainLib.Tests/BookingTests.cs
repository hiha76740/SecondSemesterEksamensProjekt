using BookRight.DomainLib.Entities;
using BookRight.DomainLib.Enums;
using BookRight.DomainLib.Exceptions;
using BookRight.DomainLib.ValueObjects;
using Xunit;

namespace BookRight.DomainLib.Tests
{
    public class TimeSlotTests
    {
        [Fact]
        public void Constructor_WithEndBeforeStart_ThrowsDomainException()
        {
            Assert.Throws<DomainException>(() =>
                new TimeSlot(DateTime.UtcNow.AddHours(2), DateTime.UtcNow.AddHours(1)));
        }

        [Fact]
        public void Duration_IsCalculatedCorrectly()
        {
            var timeSlot = new TimeSlot(Day(9), Day(9, 45));

            Assert.Equal(TimeSpan.FromMinutes(45), timeSlot.Duration);
        }

        private DateTime Day(int hour, int minute = 0)
        {
            return DateTime.UtcNow.AddDays(1).Date.AddHours(hour).AddMinutes(minute);
        }
    }

    public class BookingTests
    {
        private readonly Guid CustomerId = Guid.NewGuid();
        private readonly Guid TreatmentId = Guid.NewGuid();
        private readonly Guid TherapistId = Guid.NewGuid();
        private readonly Guid ClinicId = Guid.NewGuid();

        private TimeSlot Time(int fromHour, int toHour)
        {
            return new TimeSlot(
                DateTime.UtcNow.AddDays(1).Date.AddHours(fromHour),
                DateTime.UtcNow.AddDays(1).Date.AddHours(toHour));
        }

        private Booking CreateWithoutOverlap(
            TimeSlot? timeSlot = null,
            Guid? customerId = null,
            Guid? treatmentId = null,
            Guid? therapistId = null,
            Guid? clinicId = null,
            decimal price = 550)
        {
            return Booking.Create(
                timeSlot ?? Time(9, 10),
                customerId ?? CustomerId,
                treatmentId ?? TreatmentId,
                therapistId ?? TherapistId,
                clinicId ?? ClinicId,
                price,
                existingTherapistBookings: Array.Empty<Booking>(),
                existingClinicBookings: Array.Empty<Booking>());
        }

        [Fact]
        public void Create_WithValidData_SetsStatusToCreated()
        {
            var booking = CreateWithoutOverlap();

            Assert.Equal(BookingStatus.Created, booking.Status);
            Assert.True(booking.IsActive);
        }

        [Fact]
        public void Create_WithValidData_SetsProperties()
        {
            var booking = CreateWithoutOverlap();

            Assert.NotEqual(Guid.Empty, booking.Id);
            Assert.Equal(CustomerId, booking.CustomerId);
            Assert.Equal(TreatmentId, booking.TreatmentId);
            Assert.Equal(TherapistId, booking.TherapistId);
            Assert.Equal(ClinicId, booking.ClinicId);
            Assert.Equal(550, booking.Price);
        }

        [Fact]
        public void Create_WithEmptyCustomerId_ThrowsDomainException()
        {
            Assert.Throws<DomainException>(() =>
                CreateWithoutOverlap(customerId: Guid.Empty));
        }

        [Fact]
        public void Create_WithEmptyTreatmentId_ThrowsDomainException()
        {
            Assert.Throws<DomainException>(() =>
                CreateWithoutOverlap(treatmentId: Guid.Empty));
        }

        [Fact]
        public void Create_WithEmptyTherapistId_ThrowsDomainException()
        {
            Assert.Throws<DomainException>(() =>
                CreateWithoutOverlap(therapistId: Guid.Empty));
        }

        [Fact]
        public void Create_WithEmptyClinicId_ThrowsDomainException()
        {
            Assert.Throws<DomainException>(() =>
                CreateWithoutOverlap(clinicId: Guid.Empty));
        }

        [Fact]
        public void Create_WithNegativePrice_ThrowsDomainException()
        {
            Assert.Throws<DomainException>(() =>
                CreateWithoutOverlap(price: -100));
        }

        [Fact]
        public void Create_WithTherapistOverlap_ThrowsDomainException()
        {
            var therapistId = Guid.NewGuid();

            var existingBooking = Booking.Create(
                Time(9, 10),
                CustomerId,
                TreatmentId,
                therapistId,
                ClinicId,
                550,
                Array.Empty<Booking>(),
                Array.Empty<Booking>());

            Assert.Throws<DomainException>(() =>
                Booking.Create(
                    Time(9, 11),
                    Guid.NewGuid(),
                    TreatmentId,
                    therapistId,
                    Guid.NewGuid(),
                    550,
                    existingTherapistBookings: new[] { existingBooking },
                    existingClinicBookings: Array.Empty<Booking>()));
        }

        [Fact]
        public void Create_WithClinicOverlap_ThrowsDomainException()
        {
            var clinicId = Guid.NewGuid();

            var existingBooking = Booking.Create(
                Time(9, 10),
                CustomerId,
                TreatmentId,
                TherapistId,
                clinicId,
                550,
                Array.Empty<Booking>(),
                Array.Empty<Booking>());

            Assert.Throws<DomainException>(() =>
                Booking.Create(
                    Time(9, 11),
                    Guid.NewGuid(),
                    TreatmentId,
                    Guid.NewGuid(),
                    clinicId,
                    550,
                    existingTherapistBookings: Array.Empty<Booking>(),
                    existingClinicBookings: new[] { existingBooking }));
        }

        [Fact]
        public void Create_WithoutOverlap_CreatesBooking()
        {
            var therapistId = Guid.NewGuid();
            var clinicId = Guid.NewGuid();

            var existingBooking = Booking.Create(
                Time(9, 10),
                CustomerId,
                TreatmentId,
                therapistId,
                clinicId,
                550,
                Array.Empty<Booking>(),
                Array.Empty<Booking>());

            var newBooking = Booking.Create(
                Time(10, 11),
                Guid.NewGuid(),
                TreatmentId,
                therapistId,
                clinicId,
                550,
                existingTherapistBookings: new[] { existingBooking },
                existingClinicBookings: new[] { existingBooking });

            Assert.NotNull(newBooking);
        }

        [Fact]
        public void Create_WithCancelledOverlappingBooking_CreatesBooking()
        {
            var therapistId = Guid.NewGuid();
            var clinicId = Guid.NewGuid();

            var cancelledBooking = Booking.Create(
                Time(9, 10),
                CustomerId,
                TreatmentId,
                therapistId,
                clinicId,
                550,
                Array.Empty<Booking>(),
                Array.Empty<Booking>());

            cancelledBooking.Cancel();

            var newBooking = Booking.Create(
                Time(9, 10),
                Guid.NewGuid(),
                TreatmentId,
                therapistId,
                clinicId,
                550,
                existingTherapistBookings: new[] { cancelledBooking },
                existingClinicBookings: new[] { cancelledBooking });

            Assert.NotNull(newBooking);
        }

        [Fact]
        public void Create_WithCompletedOverlappingBooking_ThrowsDomainException()
        {
            var therapistId = Guid.NewGuid();
            var clinicId = Guid.NewGuid();

            var completedBooking = Booking.Create(
                Time(9, 10),
                CustomerId,
                TreatmentId,
                therapistId,
                clinicId,
                550,
                Array.Empty<Booking>(),
                Array.Empty<Booking>());

            completedBooking.Complete();

            Assert.Throws<DomainException>(() =>
                Booking.Create(
                    Time(9, 10),
                    Guid.NewGuid(),
                    TreatmentId,
                    therapistId,
                    clinicId,
                    550,
                    existingTherapistBookings: new[] { completedBooking },
                    existingClinicBookings: new[] { completedBooking }));
        }

        [Fact]
        public void Cancel_SetsStatusToCancelled()
        {
            var booking = CreateWithoutOverlap();

            booking.Cancel();

            Assert.Equal(BookingStatus.Cancelled, booking.Status);
            Assert.False(booking.IsActive);
        }

        [Fact]
        public void Complete_SetsStatusToCompleted()
        {
            var booking = CreateWithoutOverlap();

            booking.Complete();

            Assert.Equal(BookingStatus.Completed, booking.Status);
            Assert.True(booking.IsActive);
        }

        [Fact]
        public void NoShow_SetsStatusToNoShow()
        {
            var booking = CreateWithoutOverlap();

            booking.NoShow();

            Assert.Equal(BookingStatus.NoShow, booking.Status);
            Assert.True(booking.IsActive);
        }

        [Fact]
        public void Arrived_SetsStatusToArrived()
        {
            var booking = CreateWithoutOverlap();

            booking.Arrived();

            Assert.Equal(BookingStatus.Arrived, booking.Status);
            Assert.True(booking.IsActive);
        }

        [Fact]
        public void ChangeTreatment_WhenBookingIsCompleted_ThrowsDomainException()
        {
            var booking = CreateWithoutOverlap();
            booking.Complete();

            Assert.Throws<DomainException>(() =>
                booking.ChangeTreatment(Guid.NewGuid()));
        }

        [Fact]
        public void ChangeTreatment_WhenBookingIsCancelled_ThrowsDomainException()
        {
            var booking = CreateWithoutOverlap();
            booking.Cancel();

            Assert.Throws<DomainException>(() =>
                booking.ChangeTreatment(Guid.NewGuid()));
        }
    }
}
