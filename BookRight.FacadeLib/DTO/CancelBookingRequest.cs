namespace BookRight.FacadeLib.DTO;

// === Request DTO'er til Use Cases (commands) ===

public record CancelBookingRequest(Guid BookingId);