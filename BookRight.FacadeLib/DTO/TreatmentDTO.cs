namespace BookRight.FacadeLib.DTO;

public record TreatmentDTO(
    string Name,
    decimal Price,
    TimeSpan Duration,
    string CertificationRequired,
    int MaxParticipants
    );
