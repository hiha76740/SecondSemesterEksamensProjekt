using BookRight.DomainLib.Entities.Clinics;

namespace BookRight.ApplicationLib.Repositories;

public interface IClinicRepository
{
    Task AddAsync(Clinic clinic);
    Task<Clinic?> GetByIdAsync(Guid id);
    Task SaveAsync();
}