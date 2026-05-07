namespace BookRight.FacadeLib.Commands.Booking.DTOs;

// === Request DTO'er til Use Cases (commands) ===

public record ChangeClinicCommand(
    Guid BookingId,
    Guid ClinicId);
