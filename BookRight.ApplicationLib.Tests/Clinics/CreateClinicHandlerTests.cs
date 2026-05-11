using BookRight.ApplicationLib.Handlers.Clinics;
using BookRight.ApplicationLib.Repositories;
using BookRight.DomainLib.Entities.Clinics;
using BookRight.FacadeLib.Commands.Clinics.DTOs;
using BookRight.FacadeLib.Commands.Clinics.Interfaces;
using Moq;

namespace BookRight.ApplicationLib.Tests.Clinics;

public class CreateClinicHandlerTests
{
    /*
    private static CreateClinicCommand CreateCommand()
    {
        DateTime open = new DateTime(2030, 5, 1, 8, 0, 0);
        DateTime close = open.AddHours(8);

        return new CreateClinicCommand(
            "Ny Klinik Vejle",
            "Testgade 42",
            "7100",
            "Vejle",
            5,
            open,
            close);
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
    */

}
