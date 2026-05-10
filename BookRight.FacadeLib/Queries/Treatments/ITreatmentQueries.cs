using BookRight.FacadeLib.DTO;

namespace BookRight.FacadeLib.Queries.Treatments;

public interface ITreatmentQueries
{
    Task<TreatmentDTO?> GetByIdAsync(Guid Id);
    Task<IReadOnlyList<TreatmentDTO>> GetAllAsync();
}
