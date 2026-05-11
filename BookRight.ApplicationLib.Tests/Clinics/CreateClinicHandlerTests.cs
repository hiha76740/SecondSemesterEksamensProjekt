using BookRight.ApplicationLib.Handlers.Clinics;
using BookRight.ApplicationLib.Repositories;
using BookRight.DomainLib.Entities.Clinics;
using BookRight.DomainLib.Enums;
using BookRight.FacadeLib.Commands.Clinics.DTOs;
using BookRight.FacadeLib.Commands.Clinics.Interfaces;
using BookRight.FacadeLib.DTO;
using Moq;

namespace BookRight.ApplicationLib.Tests.Clinics;

public class CreateClinicHandlerTests
{

    private static List<OpeningHourDTO> OpeningHoursDTO = new()
    {
        new OpeningHourDTO(
                "Monday",
                new TimeOnly(8, 0, 0),
                new TimeOnly(16, 0, 0),
                false),

        new OpeningHourDTO(
                "Tuesday",
                new TimeOnly(8, 0, 0),
                new TimeOnly(16, 0, 0),
                false),

        new OpeningHourDTO(
                "Wednesday",
                new TimeOnly(8, 0, 0),
                new TimeOnly(16, 0, 0),
                false),

        new OpeningHourDTO(
                "Thursday",
                new TimeOnly(8, 0, 0),
                new TimeOnly(16, 0, 0),
                false),

        new OpeningHourDTO(
                "Friday",
                new TimeOnly(8, 0, 0),
                new TimeOnly(16, 0, 0),
                false),

        new OpeningHourDTO(
                "Saturday",
                new TimeOnly(8, 0, 0),
                new TimeOnly(16, 0, 0),
                false),

        new OpeningHourDTO(
                "Sunday",
                new TimeOnly(8, 0, 0),
                new TimeOnly(16, 0, 0),
                false)

    };

    private static CreateClinicCommand CreateCommand()
    {

        return new CreateClinicCommand(
            "Ny Klinik Vejle",
            "Testgade 42",
            "7100",
            "Vejle",
            5,
            OpeningHoursDTO);
    }

    [Fact]
    public async Task Handle_GivenValidCommand_CallsSaveAndAdd()
    {
        var mockClinicRepo = new Mock<IClinicRepository>();
        var handler = new CreateClinicHandler(mockClinicRepo.Object) as ICreateClinicHandler;

        var command = CreateCommand();

        await handler.Handle(command);

        mockClinicRepo.Verify(r => r.AddAsync(It.IsAny<Clinic>()), Times.Once);
        mockClinicRepo.Verify(r => r.SaveAsync(), Times.Once);
    }

}
