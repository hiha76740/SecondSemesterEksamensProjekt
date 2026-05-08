using BookRight.ApplicationLib.Repositories;
using BookRight.DomainLib.Exceptions;
using BookRight.FacadeLib.Commands.Therapists.DTOs;
using BookRight.FacadeLib.Commands.Therapists.Interfaces;

namespace BookRight.ApplicationLib.Handlers.Therapists;

public class RemoveClinicFromTherapistHandler(
    ITherapistRepository therapistRepository,
    IClinicRepository clinicRepository)
    : IRemoveClinicFromTherapistHandler
{
    async Task IRemoveClinicFromTherapistHandler.Handle(
        RemoveClinicFromTherapistCommand command)
    {
        var therapist = await therapistRepository
            .GetByIdAsync(command.TherapistId)
            ?? throw new NotFoundException(
                "Therapist could not be found");


        _ = await clinicRepository
            .GetByIdAsync(command.ClinicId)
            ?? throw new NotFoundException(
                "Clinic could not be found");





        therapist.AssociatedClinicIds.Remove(command.ClinicId);


        await therapistRepository.SaveAsync();
    }
}