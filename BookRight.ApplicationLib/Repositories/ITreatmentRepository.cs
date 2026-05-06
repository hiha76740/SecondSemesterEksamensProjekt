using BookRight.DomainLib.Entities.Treatments;

namespace BookRight.ApplicationLib.Repositories;

public interface ITreatmentRepository
{
    Task<Treatment?> GetByIdAsync(Guid id);
}
