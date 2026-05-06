namespace BookRight.FacadeLib.Booking.DTOs;

// === Request DTO'er til Use Cases (commands) ===

public record ChangeTreatmentCommand(
    Guid BookingId,
    Guid TreatmentId);