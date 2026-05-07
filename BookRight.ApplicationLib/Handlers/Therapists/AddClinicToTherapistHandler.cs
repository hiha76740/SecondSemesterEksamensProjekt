using BookRight.ApplicationLib.Repositories;
using BookRight.DomainLib.Exceptions;
using BookRight.FacadeLib.Commands.Therapists.DTOs;
using BookRight.FacadeLib.Commands.Therapists.Interfaces;

namespace BookRight.ApplicationLib.Handlers.Therapists;

public class AddClinicToTherapistHandler(
    ITherapistRepository therapistRepository,
    IClinicRepository clinicRepository)
    : IAddClinicToTherapistHandler
{
    async Task IAddClinicToTherapistHandler.Handle(
        AddClinicToTherapistCommand command)
    {
        var therapist = await therapistRepository
            .GetByIdAsync(command.TherapistId)
            ?? throw new NotFoundException(
                "Therapist could not be found");


        _ = await clinicRepository
            .GetByIdAsync(command.ClinicId)
            ?? throw new NotFoundException(
                "Clinic could not be found");



        therapist.AssociatedClinicIds.Add(command.ClinicId);


        await therapistRepository.SaveAsync();
    }
}