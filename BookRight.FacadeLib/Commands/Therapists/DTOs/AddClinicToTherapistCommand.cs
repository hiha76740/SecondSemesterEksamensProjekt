namespace BookRight.FacadeLib.Commands.Therapists.DTOs;

public sealed record AddClinicToTherapistCommand(
    Guid TherapistId,
    Guid ClinicId);