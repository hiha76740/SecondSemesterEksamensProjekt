using BookRight.FacadeLib.DTO;

namespace BookRight.FacadeLib.Queries.Clinics;

public interface IClinicQueries
{
    Task<ClinicDTO?> GetByIdAsync(Guid Id);
    Task<IReadOnlyList<ClinicDTO>> GetAllAsync();
}
