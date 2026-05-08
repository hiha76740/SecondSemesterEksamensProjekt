using BookRight.ApplicationLib.Handlers.Therapists;
using BookRight.ApplicationLib.Repositories;
using BookRight.DomainLib.Entities.Therapists;
using BookRight.DomainLib.Enums;
using BookRight.DomainLib.Exceptions;
using BookRight.DomainLib.ValueObjects;
using BookRight.FacadeLib.Commands.Therapists.DTOs;
using BookRight.FacadeLib.Commands.Therapists.Interfaces;
using Moq;

namespace BookRight.ApplicationLib.Tests.Handlers.Therapists;

public class AddCertificationToTherapistHandlerTests
{
    private static readonly Guid TherapistId = Guid.NewGuid();

    private static readonly Address Address =
        new("Testvej 1", "6700", "Esbjerg");

    private static readonly Email Email =
        new("test@test.dk");

    private static readonly PhoneNumber PhoneNumber =
        new("12345678");

    private static Therapist CreateTherapist()
    {
        return Therapist.Create(
            "AUTH123",
            "John Doe",
            550,
            Address,
            Email,
            PhoneNumber,
            new List<Guid>(),
            null);
    }

  
}