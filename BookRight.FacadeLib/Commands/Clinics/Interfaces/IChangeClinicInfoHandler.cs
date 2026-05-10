using BookRight.FacadeLib.Commands.Clinics.DTOs;

namespace BookRight.FacadeLib.Commands.Clinics.Interfaces;

public interface IChangeClinicInfoHandler
{
    Task Handle(ChangeClinicInfoCommand command);
}
