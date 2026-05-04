using BookRight.DomainLib.Entities.Bookings;
using BookRight.DomainLib.Enums;
using BookRight.DomainLib.Exceptions;
using BookRight.DomainLib.ValueObjects;

namespace BookRight.DomainLib.Tests;

public class BookingTests
{
    private static Guid CustomerId => Guid.Parse("4504e34a-67a5-4cba-b029-8eb0b453b80d");
    private static Guid TreatmentId => Guid.Parse("4504e34a-67a5-4cba-b029-8eb0b693b80d");
    private static Guid TherapistId => Guid.Parse("4504e34a-67a5-4cba-b029-8eb0b993b80d");
    private static Guid ClinicId => Guid.Parse("4504e34a-67a5-4cba-b029-8eb0b493c80d");

    private static Guid NewTreatmentId => Guid.Parse("4504e34a-67a5-4cba-b029-8eb0b493b81d");
    private static Guid NewTherapistId => Guid.Parse("4504e34a-67a5-4cba-b029-8eb0b123b80d");
    private static Guid NewClinicId => Guid.Parse("4504e34a-67a5-4cba-b029-8eb0b123c80d");

    private static decimal Price => 550m;
    private static decimal NegativePrice => -100m;

    private static IEnumerable<Booking> ExistingCustomerBookings => Array.Empty<Booking>();
    private static IEnumerable<Booking> ExistingTherapistBookings => Array.Empty<Booking>();
    private static IEnumerable<Booking> ExistingClinicBookings => Array.Empty<Booking>();

    private static TimeSlot CreateTimeSlot(int fromHour, int toHour)
    {
        return new TimeSlot(
            DateTime.UtcNow.AddDays(1).Date.AddHours(fromHour),
            DateTime.UtcNow.AddDays(1).Date.AddHours(toHour));
    }

    private static Booking CreateWithoutOverlap(
        TimeSlot? timeSlot = null,
        Guid? customerId = null,
        Guid? treatmentId = null,
        Guid? therapistId = null,
        Guid? clinicId = null,
        decimal? price = null,
        IEnumerable<Booking>? existingCustomerBookings = null,
        IEnumerable<Booking>? existingTherapistBookings = null)
    {
        return Booking.Create(
            timeSlot ?? CreateTimeSlot(9, 10),
            customerId ?? CustomerId,
            treatmentId ?? TreatmentId,
            therapistId ?? TherapistId,
            clinicId ?? ClinicId,
            price ?? Price,
            existingCustomerBookings ?? ExistingCustomerBookings,
            existingTherapistBookings ?? ExistingTherapistBookings);
    }

    // ---------------------------------------------------------
    // 1. Create tests (Creating a Booking)
    // ---------------------------------------------------------

    [Fact]
    public void Create_GivenValidData_SetsStatusToCreated()
    {
        // Act
        Booking booking = CreateWithoutOverlap();

        // Assert
        Assert.Equal(BookingStatus.Created, booking.Status);
    }

    [Fact]
    public void Create_GivenNegativePrice_CastDomainException()
    {
        // Act & Assert
        Assert.Throws<DomainException>(() =>
            CreateWithoutOverlap(price: NegativePrice));
    }

    [Fact]
    public void Create_GivenTherapistOverlap_CastDomainException()
    {
        // Arrange
        Booking existingBooking = CreateWithoutOverlap();

        TimeSlot overlappingTimeSlot = CreateTimeSlot(9, 11);

        // Act & Assert
        Assert.Throws<DomainException>(() =>
            CreateWithoutOverlap(
                timeSlot: overlappingTimeSlot,
                existingTherapistBookings: new[] { existingBooking }));
    }

    [Fact]
    public void Create_GivenNoOverlap_SetsStatusToCreated()
    {
        // Arrange
        Booking existingBooking = CreateWithoutOverlap();

        TimeSlot newTimeSlot = CreateTimeSlot(10, 11);

        // Act
        Booking newBooking = CreateWithoutOverlap(
            timeSlot: newTimeSlot,
            existingTherapistBookings: new[] { existingBooking });

        // Assert
        Assert.Equal(BookingStatus.Created, newBooking.Status);
    }

    [Fact]
    public void Create_GivenCancelledOverlappingBooking_SetsStatusToCreated()
    {
        // Arrange
        Booking cancelledBooking = CreateWithoutOverlap();
        cancelledBooking.Cancel();

        TimeSlot overlappingTimeSlot = CreateTimeSlot(9, 10);

        // Act
        Booking newBooking = CreateWithoutOverlap(
            timeSlot: overlappingTimeSlot,
            existingTherapistBookings: new[] { cancelledBooking });

        // Assert
        Assert.Equal(BookingStatus.Created, newBooking.Status);
    }

    [Fact]
    public void Create_GivenCompletedOverlappingBooking_CastDomainException()
    {
        // Arrange
        Booking completedBooking = CreateWithoutOverlap();
        completedBooking.Complete();

        TimeSlot overlappingTimeSlot = CreateTimeSlot(9, 10);

        // Act & Assert
        Assert.Throws<DomainException>(() =>
            CreateWithoutOverlap(
                timeSlot: overlappingTimeSlot,
                existingTherapistBookings: new[] { completedBooking }));
    }

    // ---------------------------------------------------------
    // 2. ChangeTime tests (Changing time on Booking)
    // ---------------------------------------------------------

    [Fact]
    public void ChangeTime_GivenValidTimeSlot_SetsNewTimeSlot()
    {
        // Arrange
        Booking booking = CreateWithoutOverlap();

        TimeSlot newTimeSlot = CreateTimeSlot(12, 13);

        // Act
        booking.ChangeTime(
            newTimeSlot,
            ExistingCustomerBookings,
            ExistingTherapistBookings);

        // Assert
        Assert.Equal(newTimeSlot, booking.TimeSlot);
    }

    [Fact]
    public void ChangeTime_GivenCompletedBooking_CastDomainException()
    {
        // Arrange
        Booking booking = CreateWithoutOverlap();

        TimeSlot newTimeSlot = CreateTimeSlot(12, 13);

        booking.Complete();

        // Act & Assert
        Assert.Throws<DomainException>(() =>
            booking.ChangeTime(
                newTimeSlot,
                ExistingCustomerBookings,
                ExistingTherapistBookings));
    }

    [Fact]
    public void ChangeTime_GivenTherapistOverlap_CastDomainException()
    {
        // Arrange
        Booking booking = CreateWithoutOverlap();

        TimeSlot newTimeSlot = CreateTimeSlot(12, 13);

        Booking existingBooking = CreateWithoutOverlap(timeSlot: newTimeSlot);

        TimeSlot expectedTimeSlot = booking.TimeSlot;

        // Act & Assert
        Assert.Throws<DomainException>(() =>
            booking.ChangeTime(
                newTimeSlot,
                ExistingCustomerBookings,
                new[] { existingBooking }));

        Assert.Equal(expectedTimeSlot, booking.TimeSlot);
    }

    // ---------------------------------------------------------
    // 3. ChangeTreatment tests (Changing treatment on Booking)
    // ---------------------------------------------------------

    [Fact]
    public void ChangeTreatment_GivenNewTreatmentId_SetsTreatmentId()
    {
        // Arrange
        Booking booking = CreateWithoutOverlap();

        // Act
        booking.ChangeTreatment(NewTreatmentId);

        // Assert
        Assert.Equal(NewTreatmentId, booking.TreatmentId);
    }

    [Fact]
    public void ChangeTreatment_GivenSameTreatmentId_CastDomainException()
    {
        // Arrange
        Booking booking = CreateWithoutOverlap();

        // Act & Assert
        Assert.Throws<DomainException>(() =>
            booking.ChangeTreatment(TreatmentId));
    }

    [Fact]
    public void ChangeTreatment_GivenCompletedBooking_CastDomainException()
    {
        // Arrange
        Booking booking = CreateWithoutOverlap();
        booking.Complete();

        // Act & Assert
        Assert.Throws<DomainException>(() =>
            booking.ChangeTreatment(NewTreatmentId));
    }

    [Fact]
    public void ChangeTreatment_GivenCancelledBooking_CastDomainException()
    {
        // Arrange
        Booking booking = CreateWithoutOverlap();
        booking.Cancel();

        // Act & Assert
        Assert.Throws<DomainException>(() =>
            booking.ChangeTreatment(NewTreatmentId));
    }

    // ---------------------------------------------------------
    // 4. Complete tests (Completing a Booking)
    // ---------------------------------------------------------

    [Fact]
    public void Complete_GivenCreatedBooking_SetsStatusToCompleted()
    {
        // Arrange
        Booking booking = CreateWithoutOverlap();

        // Act
        booking.Complete();

        // Assert
        Assert.Equal(BookingStatus.Completed, booking.Status);
    }

    [Fact]
    public void Complete_GivenCompletedBooking_CastDomainException()
    {
        // Arrange
        Booking booking = CreateWithoutOverlap();
        booking.Complete();

        // Act & Assert
        Assert.Throws<DomainException>(() =>
            booking.Complete());
    }

    // ---------------------------------------------------------
    // 5. Arrived tests (Marking a Booking as Arrived)
    // ---------------------------------------------------------

    [Fact]
    public void Arrived_GivenCreatedBooking_SetsStatusToArrived()
    {
        // Arrange
        Booking booking = CreateWithoutOverlap();

        // Act
        booking.Arrived();

        // Assert
        Assert.Equal(BookingStatus.Arrived, booking.Status);
    }

    [Fact]
    public void Arrived_GivenCompletedBooking_CastDomainException()
    {
        // Arrange
        Booking booking = CreateWithoutOverlap();
        booking.Complete();

        // Act & Assert
        Assert.Throws<DomainException>(() =>
            booking.Arrived());
    }

    // ---------------------------------------------------------
    // 6. ChangeTherapist tests (Changing therapist on Booking)
    // ---------------------------------------------------------

    [Fact]
    public void ChangeTherapist_GivenCompletedBooking_CastDomainException()
    {
        // Arrange
        Booking booking = CreateWithoutOverlap();
        booking.Complete();

        // Act & Assert
        Assert.Throws<DomainException>(() =>
            booking.ChangeTherapist(NewTherapistId, ExistingTherapistBookings));
    }

    [Fact]
    public void ChangeTherapist_GivenNewTherapistWithoutOverlap_SetsTherapistId()
    {
        // Arrange
        Booking booking = CreateWithoutOverlap();

        // Act
        booking.ChangeTherapist(NewTherapistId, ExistingTherapistBookings);

        // Assert
        Assert.Equal(NewTherapistId, booking.TherapistId);
    }

    [Fact]
    public void ChangeTherapist_GivenNewTherapistWithOverlap_CastDomainException()
    {
        // Arrange
        Booking booking = CreateWithoutOverlap();

        Booking existingBooking = CreateWithoutOverlap(
            therapistId: NewTherapistId,
            timeSlot: booking.TimeSlot);

        // Act & Assert
        Assert.Throws<DomainException>(() =>
            booking.ChangeTherapist(
                NewTherapistId,
                new[] { existingBooking }));
    }

    // ---------------------------------------------------------
    // 7. ChangeClinic tests (Changing clinic on Booking)
    // ---------------------------------------------------------

    [Fact]
    public void ChangeClinic_GivenCompletedBooking_CastDomainException()
    {
        // Arrange
        Booking booking = CreateWithoutOverlap();
        booking.Complete();

        // Act & Assert
        Assert.Throws<DomainException>(() =>
            booking.ChangeClinic(NewClinicId, ExistingClinicBookings));
    }

    [Fact]
    public void ChangeClinic_GivenSameClinic_CastDomainException()
    {
        // Arrange
        Booking booking = CreateWithoutOverlap();

        booking.ChangeClinic(NewClinicId, ExistingClinicBookings);

        // Act & Assert
        Assert.Throws<DomainException>(() =>
            booking.ChangeClinic(NewClinicId, ExistingClinicBookings));
    }

    [Fact]
    public void ChangeClinic_GivenNewClinicId_SetsClinicId()
    {
        // Arrange
        Booking booking = CreateWithoutOverlap();

        // Act
        booking.ChangeClinic(NewClinicId, ExistingClinicBookings);

        // Assert
        Assert.Equal(NewClinicId, booking.ClinicId);
    }

    // ---------------------------------------------------------
    // 8. Cancel tests (Cancelling a Booking)
    // ---------------------------------------------------------

    [Fact]
    public void Cancel_GivenCreatedBooking_SetsStatusToCancelled()
    {
        // Arrange
        Booking booking = CreateWithoutOverlap();

        // Act
        booking.Cancel();

        // Assert
        Assert.Equal(BookingStatus.Cancelled, booking.Status);
    }

    [Fact]
    public void Cancel_GivenCancelledBooking_CastDomainException()
    {
        // Arrange
        Booking booking = CreateWithoutOverlap();
        booking.Cancel();

        // Act & Assert
        Assert.Throws<DomainException>(() =>
            booking.Cancel());
    }

    // ---------------------------------------------------------
    // 9. NoShow tests (Marking a Booking as NoShow)
    // ---------------------------------------------------------

    [Fact]
    public void NoShow_GivenCreatedBooking_SetsStatusToNoShow()
    {
        // Arrange
        Booking booking = CreateWithoutOverlap();

        // Act
        booking.NoShow();

        // Assert
        Assert.Equal(BookingStatus.NoShow, booking.Status);
    }

    [Fact]
    public void NoShow_GivenCompletedBooking_CastDomainException()
    {
        // Arrange
        Booking booking = CreateWithoutOverlap();
        booking.Complete();

        // Act & Assert
        Assert.Throws<DomainException>(() =>
            booking.NoShow());
    }
}