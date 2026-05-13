namespace BookRight.FacadeLib.DTO;

public record BookingDTO(
    Guid BookingId,
    Guid TreatmentId,
    Guid TherapistId,
    Guid ClinicId,
    DateTime From,
    DateTime To,
    decimal Price,
    int ParticipantLimit,
    IReadOnlyList<Guid> Participants,
    string Status,
    string DiscountTypeUsed
    );