using BookRight.FacadeLib.DTO;
using BookRight.FacadeLib.Queries.Clinics;
using BookRight.InfrastructureLib.Persistance.DbContextFiles;
using Microsoft.EntityFrameworkCore;

namespace BookRight.InfrastructureLib.QueryHandlers.Clinics;

public class ClinicQueryHandlerIMPL(BookRightDbContext db) : IClinicQueries
{
    async Task<IReadOnlyList<ClinicDTO>> IClinicQueries.GetAllAsync()
    {
        throw new NotImplementedException();
    }

    async Task<ClinicDTO?> IClinicQueries.GetByIdAsync(Guid Id)
    {
        throw new NotImplementedException();
    }
}
