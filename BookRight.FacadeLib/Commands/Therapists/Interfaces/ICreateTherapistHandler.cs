using BookRight.FacadeLib.Commands.Therapists.DTOs;

namespace BookRight.FacadeLib.Commands.Therapists.Interfaces;

public interface ICreateTherapistHandler
{
    Task Handle(CreateTherapistCommand command);
}
