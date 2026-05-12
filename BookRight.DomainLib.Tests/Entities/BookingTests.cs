using BookRight.DomainLib.Entities.Bookings;
using BookRight.DomainLib.Enums;
using BookRight.DomainLib.Exceptions;
using BookRight.DomainLib.ValueObjects;

namespace BookRight.DomainLib.Tests.Entities;

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

    private static TimeSlot CreateTimeSlot(int fromHour, int toHour)
    {
        return new TimeSlot(
            DateTime.UtcNow.AddDays(1).Date.AddHours(fromHour),
            DateTime.UtcNow.AddDays(1).Date.AddHours(toHour));
    }

    private static Booking CreateWithoutOverlap(
        Guid? customerId,
        TimeSlot? timeSlot = null,
        Guid? treatmentId = null,
        Guid? therapistId = null,
        Guid? clinicId = null,
        decimal? price = null,
        int? participantLimit = null
        )
    {
        return Booking.Create(
            timeSlot ?? CreateTimeSlot(9, 10),
            treatmentId ?? TreatmentId,
            therapistId ?? TherapistId,
            clinicId ?? ClinicId,
            price ?? Price,
            participantLimit ?? 1,
            DiscountTypes.None,
            customerId);
    }

    // ---------------------------------------------------------
    // 1. Create tests (Creating a Booking)
    // ---------------------------------------------------------

    [Fact]
    public void Create_GivenMultiParticipantBooking_SetsStatusToCreate()
    {
        // Act
        var booking = CreateWithoutOverlap(participantLimit: 5, customerId: null);

        // Assert
        Assert.Equal(BookingStatus.Created, booking.Status);
        Assert.Empty(booking.Participants);
    }

    [Fact]
    public void Create_GivenSingleParticipantBooking_SetsStatusToCreated()
    {
        // Act
        Booking booking = CreateWithoutOverlap(CustomerId);

        // Assert
        Assert.Equal(BookingStatus.Created, booking.Status);
        Assert.Contains<Guid>(CustomerId, booking.Participants);
    }

    [Fact]
    public void Create_GivenNegativePrice_CastDomainException()
    {
        // Act & Assert
        Assert.Throws<DomainException>(() => CreateWithoutOverlap(CustomerId, price: NegativePrice));
    }

    [Fact]
    public void Create_GivenNoOverlap_SetsStatusToCreated()
    {
        // Arrange
        TimeSlot newTimeSlot = CreateTimeSlot(10, 11);

        // Act
        Booking newBooking = CreateWithoutOverlap(CustomerId, timeSlot: newTimeSlot);

        // Assert
        Assert.Equal(BookingStatus.Created, newBooking.Status);
    }

    [Fact]
    public void Create_GivenCancelledOverlappingBooking_SetsStatusToCreated()
    {
        // Arrange
        Booking cancelledBooking = CreateWithoutOverlap(CustomerId);
        cancelledBooking.Cancel();

        TimeSlot overlappingTimeSlot = CreateTimeSlot(9, 10);

        // Act
        Booking newBooking = CreateWithoutOverlap(CustomerId, timeSlot: overlappingTimeSlot);

        // Assert
        Assert.Equal(BookingStatus.Created, newBooking.Status);
    }

    // ---------------------------------------------------------
    // 2. ChangeTime tests (Changing time on Booking)
    // ---------------------------------------------------------

    [Fact]
    public void ChangeTime_GivenValidTimeSlot_SetsNewTimeSlot()
    {
        // Arrange
        Booking booking = CreateWithoutOverlap(CustomerId);

        TimeSlot newTimeSlot = CreateTimeSlot(12, 13);

        // Act
        booking.ChangeTime(
            newTimeSlot);

        // Assert
        Assert.Equal(newTimeSlot, booking.Time);
    }

    [Fact]
    public void ChangeTime_GivenCompletedBooking_CastDomainException()
    {
        // Arrange
        Booking booking = CreateWithoutOverlap(CustomerId);

        TimeSlot newTimeSlot = CreateTimeSlot(12, 13);

        booking.Complete();

        // Act & Assert
        Assert.Throws<DomainException>(() => booking.ChangeTime(newTimeSlot));
    }

    // ---------------------------------------------------------
    // 3. ChangeTreatment tests (Changing treatment on Booking)
    // ---------------------------------------------------------

    [Fact]
    public void ChangeTreatment_GivenNewTreatmentId_SetsTreatmentId()
    {
        // Arrange
        Booking booking = CreateWithoutOverlap(CustomerId);

        // Act
        booking.ChangeTreatment(NewTreatmentId);

        // Assert
        Assert.Equal(NewTreatmentId, booking.TreatmentId);
    }

    [Fact]
    public void ChangeTreatment_GivenSameTreatmentId_CastDomainException()
    {
        // Arrange
        Booking booking = CreateWithoutOverlap(CustomerId);

        // Act & Assert
        Assert.Throws<DomainException>(() => booking.ChangeTreatment(TreatmentId));
    }

    [Fact]
    public void ChangeTreatment_GivenCompletedBooking_CastDomainException()
    {
        // Arrange
        Booking booking = CreateWithoutOverlap(CustomerId);
        booking.Complete();

        // Act & Assert
        Assert.Throws<DomainException>(() => booking.ChangeTreatment(NewTreatmentId));
    }

    [Fact]
    public void ChangeTreatment_GivenCancelledBooking_CastDomainException()
    {
        // Arrange
        Booking booking = CreateWithoutOverlap(CustomerId);
        booking.Cancel();

        // Act & Assert
        Assert.Throws<DomainException>(() => booking.ChangeTreatment(NewTreatmentId));
    }

    // ---------------------------------------------------------
    // 4. Complete tests (Completing a Booking)
    // ---------------------------------------------------------

    [Fact]
    public void Complete_GivenCreatedBooking_SetsStatusToCompleted()
    {
        // Arrange
        Booking booking = CreateWithoutOverlap(CustomerId);

        // Act
        booking.Complete();

        // Assert
        Assert.Equal(BookingStatus.Completed, booking.Status);
    }

    [Fact]
    public void Complete_GivenCompletedBooking_CastDomainException()
    {
        // Arrange
        Booking booking = CreateWithoutOverlap(CustomerId);
        booking.Complete();

        // Act & Assert
        Assert.Throws<DomainException>(() => booking.Complete());
    }

    // ---------------------------------------------------------
    // 5. Arrived tests (Marking a Booking as Arrived)
    // ---------------------------------------------------------

    [Fact]
    public void Arrived_GivenCreatedBooking_SetsStatusToArrived()
    {
        // Arrange
        Booking booking = CreateWithoutOverlap(CustomerId);

        // Act
        booking.Arrived();

        // Assert
        Assert.Equal(BookingStatus.Arrived, booking.Status);
    }

    [Fact]
    public void Arrived_GivenCompletedBooking_CastDomainException()
    {
        // Arrange
        Booking booking = CreateWithoutOverlap(CustomerId);
        booking.Complete();

        // Act & Assert
        Assert.Throws<DomainException>(() => booking.Arrived());
    }

    // ---------------------------------------------------------
    // 6. ChangeTherapist tests (Changing therapist on Booking)
    // ---------------------------------------------------------

    [Fact]
    public void ChangeTherapist_GivenCompletedBooking_CastDomainException()
    {
        // Arrange
        Booking booking = CreateWithoutOverlap(CustomerId);
        booking.Complete();

        // Act & Assert
        Assert.Throws<DomainException>(() => booking.ChangeTherapist(NewTherapistId));
    }

    [Fact]
    public void ChangeTherapist_GivenNewTherapistWithoutOverlap_SetsTherapistId()
    {
        // Arrange
        Booking booking = CreateWithoutOverlap(CustomerId);

        // Act
        booking.ChangeTherapist(NewTherapistId);

        // Assert
        Assert.Equal(NewTherapistId, booking.TherapistId);
    }

    // ---------------------------------------------------------
    // 7. ChangeClinic tests (Changing clinic on Booking)
    // ---------------------------------------------------------

    [Fact]
    public void ChangeClinic_GivenCompletedBooking_CastDomainException()
    {
        // Arrange
        Booking booking = CreateWithoutOverlap(CustomerId);
        booking.Complete();

        // Act & Assert
        Assert.Throws<DomainException>(() => booking.ChangeClinic(NewClinicId));
    }

    [Fact]
    public void ChangeClinic_GivenSameClinic_CastDomainException()
    {
        // Arrange
        Booking booking = CreateWithoutOverlap(CustomerId);

        booking.ChangeClinic(NewClinicId);

        // Act & Assert
        Assert.Throws<DomainException>(() => booking.ChangeClinic(NewClinicId));
    }

    [Fact]
    public void ChangeClinic_GivenNewClinicId_SetsClinicId()
    {
        // Arrange
        Booking booking = CreateWithoutOverlap(CustomerId);

        // Act
        booking.ChangeClinic(NewClinicId);

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
        Booking booking = CreateWithoutOverlap(CustomerId);

        // Act
        booking.Cancel();

        // Assert
        Assert.Equal(BookingStatus.Cancelled, booking.Status);
    }

    [Fact]
    public void Cancel_GivenCancelledBooking_CastDomainException()
    {
        // Arrange
        Booking booking = CreateWithoutOverlap(CustomerId);
        booking.Cancel();

        // Act & Assert
        Assert.Throws<DomainException>(() => booking.Cancel());
    }

    // ---------------------------------------------------------
    // 9. NoShow tests (Marking a Booking as NoShow)
    // ---------------------------------------------------------

    [Fact]
    public void NoShow_GivenCreatedBooking_SetsStatusToNoShow()
    {
        // Arrange
        Booking booking = CreateWithoutOverlap(CustomerId);

        // Act
        booking.NoShow();

        // Assert
        Assert.Equal(BookingStatus.NoShow, booking.Status);
    }

    [Fact]
    public void NoShow_GivenCompletedBooking_CastDomainException()
    {
        // Arrange
        Booking booking = CreateWithoutOverlap(CustomerId);
        booking.Complete();

        // Act & Assert
        Assert.Throws<DomainException>(() => booking.NoShow());
    }
}