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


   
}