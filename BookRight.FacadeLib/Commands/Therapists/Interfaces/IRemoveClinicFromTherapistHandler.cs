using System;
using BookRight.FacadeLib.Commands.Therapists.DTOs;

namespace BookRight.FacadeLib.Commands.Therapists.Interfaces;

public interface IRemoveClinicFromTherapistHandler
{
    Task Handle(RemoveClinicFromTherapistCommand command);
}