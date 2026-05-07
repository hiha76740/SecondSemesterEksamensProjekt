using BookRight.ApplicationLib.Repositories;
using BookRight.DomainLib.Exceptions;
using BookRight.FacadeLib.Commands.Therapists.DTOs;
using BookRight.FacadeLib.Commands.Therapists.Interfaces;

namespace BookRight.ApplicationLib.Handlers.Therapists;

public class AddClinicToTherapistHandler(
    ITherapistRepository therapistRepository,
    IClinicRepository clinicRepository)
    : IAddClinicToTherapistHandler
{
    
}