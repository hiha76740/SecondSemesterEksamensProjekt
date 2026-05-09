using BookRight.FacadeLib.Commands.Therapists.DTOs;

namespace BookRight.FacadeLib.Commands.Therapists.Interfaces;

public interface IChangeTherapistInfoHandler
{
    Task Handle(ChangeTherapistInfoCommand command);
}