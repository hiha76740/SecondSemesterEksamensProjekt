using BookRight.FacadeLib.DTO;

namespace BookRight.FacadeLib.Queries.CertificationTypes;

public interface ICertificationTypeQueries
{
    IReadOnlyList<CertificationTypeDTO> GetAll();
}
