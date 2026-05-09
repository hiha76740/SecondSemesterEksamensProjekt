using BookRight.ApplicationLib.Repositories;
using BookRight.DomainLib.Entities.Clinics;
using BookRight.DomainLib.ValueObjects;
using BookRight.FacadeLib.Commands.Clinics.DTOs;
using BookRight.FacadeLib.Commands.Clinics.Interfaces;

namespace BookRight.ApplicationLib.Handlers.Clinics;

public class CreateClinicHandler(IClinicRepository clinicRepository) : ICreateClinicHandler
{
    async Task ICreateClinicHandler.Handle(CreateClinicCommand command)
    {
        var address = new Address(command.Street, command.PostalCode, command.City);
        var openingHours = new OpeningHours(command.OpeningHour, command.CloseingHour);

        var clinic = Clinic.Create(command.Name, command.TreatmentRoomLimit, openingHours, address);

        await clinicRepository.AddAsync(clinic);
        await clinicRepository.SaveAsync();
    }
}
