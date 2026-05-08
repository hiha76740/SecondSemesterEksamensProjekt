namespace BookRight.FacadeLib.Commands.Therapists.DTOs;

public sealed record RemoveClinicFromTherapistCommand(Guid TherapistId, Guid ClinicId);