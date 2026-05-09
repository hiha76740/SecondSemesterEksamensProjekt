namespace BookRight.FacadeLib.Commands.Booking.DTOs;

// === Request DTO'er til Use Cases (commands) ===

public record CreateBookingCommand(
    Guid TreatmentId,
    Guid TherapistId,
    Guid ClinicId,
    DateTime From,
    DateTime To,
    Guid? CustomerId = null);