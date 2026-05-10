namespace BookRight.FacadeLib.Commands.Booking.DTOs;

public record AddParticipantCommand(Guid BookingId, Guid CustomerId);
