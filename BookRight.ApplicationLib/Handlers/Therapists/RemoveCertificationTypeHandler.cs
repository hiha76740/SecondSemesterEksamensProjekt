using BookRight.ApplicationLib.Repositories;
using BookRight.DomainLib.Enums;
using BookRight.DomainLib.Exceptions;
using BookRight.FacadeLib.Commands.Therapists.DTOs;
using BookRight.FacadeLib.Commands.Therapists.Interfaces;

namespace BookRight.ApplicationLib.Handlers.Therapists;

public class RemoveCertificationTypeHandler : IRemoveCertificationTypeHandler
{
    private readonly ITherapistRepository _therapistRepository;

    public RemoveCertificationTypeHandler(
        ITherapistRepository therapistRepository)
    {
        _therapistRepository = therapistRepository;
    }


    async Task IRemoveCertificationTypeHandler.Handle(
        RemoveCertificationTypeCommand command)
    {
        var therapist = await _therapistRepository
            .GetByIdAsync(command.TherapistId)
            ?? throw new NotFoundException(
                "Therapist could not be found");


        bool certificationTypeExsists =
            Enum.TryParse<CertificationTypes>(
                command.CertificationType,
                out var certificationType);

        if (certificationTypeExsists == false)
            throw new NotFoundException(
                "Certification Type not found");


        therapist.RemoveCertificationType(
            certificationType);


        await _therapistRepository.SaveAsync();
    }
}