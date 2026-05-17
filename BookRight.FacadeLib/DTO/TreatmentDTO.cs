namespace BookRight.FacadeLib.DTO;

public record TreatmentDTO(
    Guid Id,
    string Name,
    decimal Price,
    TimeSpan Duration,
    string CertificationRequired,
    int MaxParticipants
    );
