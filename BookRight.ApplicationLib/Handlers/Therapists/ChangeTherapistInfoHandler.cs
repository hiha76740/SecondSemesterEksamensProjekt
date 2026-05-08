using BookRight.ApplicationLib.Repositories;
using BookRight.DomainLib.Enums;
using BookRight.DomainLib.Exceptions;
using BookRight.DomainLib.ValueObjects;
using BookRight.FacadeLib.Commands.Therapists.DTOs;
using BookRight.FacadeLib.Commands.Therapists.Interfaces;

namespace BookRight.ApplicationLib.Handlers.Therapists;

public class ChangeTherapistInfoHandler(ITherapistRepository therapistRepository) : IChangeTherapistInfoHandler
{
    async Task IChangeTherapistInfoHandler.Handle(ChangeTherapistInfoCommand command)
    {
        bool changesMade = false;

        var therapist = await therapistRepository.GetByIdAsync(command.TherapistId)
          ?? throw new NotFoundException("Therapist could not be found");

        var address = new Address(
          command.Street,
          command.PostalCode,
          command.City);

        var email = new Email(
          command.EmailAddress);

        var phoneNumber = new PhoneNumber(
          command.PhoneNumber);

        if (therapist.Name != command.Name)
        {
            therapist.ChangeName(command.Name);
            changesMade = true;
        }

        if (therapist.HourlyRate != command.HourlyRate)
        {
            therapist.ChangeHourlyRate(command.HourlyRate);
            changesMade = true;
        }

        if (therapist.Address != address)
        {
            therapist.ChangeAddress(address);
            changesMade = true;
        }

        if (therapist.Email != email)
        {
            therapist.ChangeEmail(email);
            changesMade = true;
        }

        if (therapist.PhoneNumber != phoneNumber)
        {
            therapist.ChangePhoneNumber(phoneNumber);
            changesMade = true;
        }

        // Gennemgår alle certificeringer fra commandet og sikrer:
        // 1. At certificeringen findes som en gyldig enum-værdi
        // 2. At certificeringen bliver tilføjet til behandleren,
        //    hvis den ikke allerede findes
        foreach (var certification in command.Certifications)
        {
            bool exsists = Enum.TryParse<CertificationTypes>(
              certification,
              out var certificationType);

            // Kaster exception hvis certificeringen ikke findes i enum'en
            if (exsists == false)
                throw new NotFoundException(
                  $"Certification type {certification} not found");

            // Tilføjer kun certificeringen hvis behandleren ikke allerede har den
            if (therapist.CertificationTypes.Contains(certificationType) == false)
            {
                therapist.AddCertificationType(certificationType);
                changesMade = true;
            }
        }
        
        // Gennemgår behandlerens nuværende certificeringer og fjerner:
        // certificeringer som ikke længere findes i commandet
        foreach (var certification in therapist.CertificationTypes.ToList())
        {
            if (command.Certifications.Contains(certification.ToString()) == false)
            {
                therapist.RemoveCertificationType(certification);
                changesMade = true;
            }
        }

        if (changesMade == true)
            await therapistRepository.SaveAsync();
    }
}