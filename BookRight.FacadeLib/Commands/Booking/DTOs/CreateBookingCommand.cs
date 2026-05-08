namespace BookRight.FacadeLib.Commands.Booking.DTOs;

// === Request DTO'er til Use Cases (commands) ===

public record CreateBookingCommand(
    Guid CustomerId,
    Guid TreatmentId,
    Guid TherapistId,
    Guid ClinicId,
    DateTime From,
    DateTime To);