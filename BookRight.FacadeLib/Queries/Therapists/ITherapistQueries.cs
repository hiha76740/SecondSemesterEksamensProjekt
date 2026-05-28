using BookRight.FacadeLib.DTO;

namespace BookRight.FacadeLib.Queries.Therapists;

public interface ITherapistQueries
{
    Task<TherapistDTO?> GetByIdAsync(Guid Id);
    Task<IReadOnlyList<TherapistDTO>> GetAllAsync();

    Task<IReadOnlyList<TherapistDTO>> SearchTherapistAsync(string filter);
}
