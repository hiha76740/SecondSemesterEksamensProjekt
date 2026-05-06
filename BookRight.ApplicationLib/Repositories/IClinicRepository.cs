using BookRight.DomainLib.Entities.Clinics;

namespace BookRight.ApplicationLib.Repositories;

public interface IClinicRepository
{
    Task<Clinic?> GetByIdAsync(Guid id);
}