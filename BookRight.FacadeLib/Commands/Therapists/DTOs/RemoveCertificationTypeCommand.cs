namespace BookRight.FacadeLib.Commands.Therapists.DTOs;

public sealed record RemoveCertificationTypeCommand(Guid TherapistId, string CertificationType);