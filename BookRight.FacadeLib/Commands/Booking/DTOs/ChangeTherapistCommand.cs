namespace BookRight.FacadeLib.Commands.Booking.DTOs;

// === Request DTO'er til Use Cases (commands) ===

public record ChangeTherapistCommand(Guid BookingId, Guid TherapistId);