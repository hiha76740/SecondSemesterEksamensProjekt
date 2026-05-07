namespace BookRight.FacadeLib.DTO;

public sealed record AddCertificationTypeCommand(
    Guid TherapistId,
    CertificationTypes CertificationType);