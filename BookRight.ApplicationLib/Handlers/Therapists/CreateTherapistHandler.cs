using BookRight.ApplicationLib.Repositories;
using BookRight.DomainLib.Entities.Therapists;
using BookRight.DomainLib.Enums;
using BookRight.DomainLib.Exceptions;
using BookRight.DomainLib.ValueObjects;
using BookRight.FacadeLib.Commands.Therapists.DTOs;
using BookRight.FacadeLib.Commands.Therapists.Interfaces;

namespace BookRight.ApplicationLib.Handlers.Therapists;

public class CreateTherapistHandler : ICreateTherapistHandler
{
    private readonly ITherapistRepository _therapistRepository;

    public CreateTherapistHandler(
        ITherapistRepository therapistRepository)
    {
        _therapistRepository = therapistRepository;
    }


    async Task ICreateTherapistHandler.Handle(
        CreateTherapistCommand command)
    {
        var address = new Address(
            command.Street,
            command.PostalCode,
            command.City);

        var email = new Email(
            command.EmailAddress);

        var phoneNumber = new PhoneNumber(
            command.PhoneNumber);

        List<CertificationTypes>? certifications = null;

        if (command.Certifications.Count > 0)
        {
            certifications = new();

            foreach (var certification in command.Certifications)
            {
                bool exsists = Enum.TryParse<CertificationTypes>(certification, out CertificationTypes certificationTypes);

                if (exsists == false)
                    throw new NotFoundException($"Unable to find cerification {certification}");

                certifications.Add(certificationTypes);
            }
        }

        var therapist = Therapist.Create(
            command.AuthorizationNumber,
            command.Name,
            command.HourlyRate,
            address,
            email,
            phoneNumber,
            command.AssociatedClinicIds,
            certifications);


        await _therapistRepository.AddAsync(therapist);

        await _therapistRepository.SaveAsync();
    }
}