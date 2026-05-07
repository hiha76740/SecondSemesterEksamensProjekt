namespace BookRight.FacadeLib.Commands.Therapists.DTOs;

public sealed record AddCertificationTypeCommand(
    Guid TherapistId,
    string CertificationType);