using BookRight.ApplicationLib.Repositories;
using BookRight.FacadeLib.DTO;
using BookRight.FacadeLib.Handlers;

namespace BookRight.ApplicationLib.Handlers.Therapists;

public class AddCertificationTypeHandler : IAddCertificationTypeHandler
{
    private readonly ITherapistRepository _therapistRepository;

    public AddCertificationTypeHandler(
        ITherapistRepository therapistRepository)
    {
        _therapistRepository = therapistRepository;
    }
    async Task IAddCertificationTypeHandler.Handle(
       AddCertificationTypeCommand command)
    {
        var therapist = await _therapistRepository
            .GetByIdAsync(command.TherapistId)
            ?? throw new ApplicationException(
                "Therapist could not be found");


        therapist.AddCertificationType(
            command.CertificationType);


        await _therapistRepository.SaveAsync();
    }

}