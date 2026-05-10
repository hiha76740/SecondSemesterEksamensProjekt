namespace BookRight.FacadeLib.Commands.Booking.DTOs;

public record RemoveParticipantCommand(Guid BookingId, Guid CustomerId);
