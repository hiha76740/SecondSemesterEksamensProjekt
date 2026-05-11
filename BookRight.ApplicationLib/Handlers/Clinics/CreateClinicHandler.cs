using BookRight.ApplicationLib.Repositories;
using BookRight.DomainLib.Entities.Clinics;
using BookRight.DomainLib.Enums;
using BookRight.DomainLib.Exceptions;
using BookRight.DomainLib.ValueObjects;
using BookRight.FacadeLib.Commands.Clinics.DTOs;
using BookRight.FacadeLib.Commands.Clinics.Interfaces;

namespace BookRight.ApplicationLib.Handlers.Clinics;

public class CreateClinicHandler(IClinicRepository clinicRepository) : ICreateClinicHandler
{
    async Task ICreateClinicHandler.Handle(CreateClinicCommand command)
    {
        var address = new Address(command.Street, command.PostalCode, command.City);

        List<OpeningHourInput> openingHourInputs = new();

        foreach (var openingHour in command.OpeningHours)
        {
            bool exsists = Enum.TryParse<Weekdays>(openingHour.Weekday, out var weekday);

            if (exsists == false)
                throw new NotFoundException($"Weekday {openingHour.Weekday} was not found");

            var openingHourInput = new OpeningHourInput(weekday, openingHour.OpeningTime, openingHour.ClosingTime, openingHour.IsClosed);

            openingHourInputs.Add(openingHourInput);
        }


        var clinic = Clinic.Create(command.Name, command.TreatmentRoomLimit, openingHourInputs, address);

        await clinicRepository.AddAsync(clinic);
        await clinicRepository.SaveAsync();
    }
}
