using BookRight.DomainLib.Enums;
using BookRight.FacadeLib.DTO;
using BookRight.FacadeLib.Queries.CertificationTypes;

namespace BookRight.InfrastructureLib.QueryHandlers.CertificationType;

public class CertificationTypeQueryHandlerIMPL : ICertificationTypeQueries
{
    IReadOnlyList<CertificationTypeDTO> ICertificationTypeQueries.GetAll()
    {
        return Enum.GetValues<CertificationTypes>()
                .Select(ct => new CertificationTypeDTO(ct.ToString())).ToList();
    }
}
