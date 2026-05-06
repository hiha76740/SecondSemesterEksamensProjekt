namespace BookRight.FacadeLib.Booking.DTOs;

// === Request DTO'er til Use Cases (commands) ===

public record ChangeTimeCommand(
    Guid BookingId,
    DateTime From,
    DateTime To);