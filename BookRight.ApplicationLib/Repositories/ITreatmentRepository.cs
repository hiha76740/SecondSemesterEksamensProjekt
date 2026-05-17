using BookRight.DomainLib.Entities.Treatments;

namespace BookRight.ApplicationLib.Repositories;

public interface ITreatmentRepository
{
    Task<IReadOnlyList<Treatment>> GetAllAsync();
    Task<Treatment?> GetByIdAsync(Guid id);
}
