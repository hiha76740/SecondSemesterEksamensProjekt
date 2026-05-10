using BookRight.ApplicationLib.Repositories;
using BookRight.DomainLib.Entities.Therapists;
using BookRight.DomainLib.Enums;
using BookRight.DomainLib.Exceptions;
using BookRight.DomainLib.ValueObjects;
using BookRight.FacadeLib.Commands.Therapists.DTOs;
using BookRight.FacadeLib.Commands.Therapists.Interfaces;

namespace BookRight.ApplicationLib.Handlers.Therapists;

public class CreateTherapistHandler(ITherapistRepository therapistRepository, IClinicRepository clinicRepository) : ICreateTherapistHandler
{
    async Task ICreateTherapistHandler.Handle(CreateTherapistCommand command)
    {
        var email = new Email(command.EmailAddress);
        var phoneNumber = new PhoneNumber(command.PhoneNumber);
        var address = new Address(
            command.Street,
            command.PostalCode,
            command.City);   

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

        foreach (var clinicId in command.AssociatedClinicIds)
        {
            _ = clinicRepository.GetByIdAsync(clinicId)
                ?? throw new NotFoundException($"Clinic with id {clinicId} not found");
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


        await therapistRepository.AddAsync(therapist);

        await therapistRepository.SaveAsync();
    }
}