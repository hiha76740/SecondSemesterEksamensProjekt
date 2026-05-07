namespace BookRight.FacadeLib.DTO;

public sealed record AddCertificationTypeCommand(
    Guid TherapistId,
    string CertificationType);