using BookRight.FacadeLib.DTO;
using BookRight.FacadeLib.Queries.Clinics;
using BookRight.InfrastructureLib.Persistance.DbContextFiles;
using Microsoft.EntityFrameworkCore;

namespace BookRight.InfrastructureLib.QueryHandlers.Clinics;

public class ClinicQueryHandlerIMPL(BookRightDbContext db) : IClinicQueries
{
    async Task<IReadOnlyList<ClinicDTO>> IClinicQueries.GetAllAsync()
    {
        return await db.Clinics
            .AsNoTracking()
            .Select(c => new ClinicDTO(
                c.Name,
                c.TreatmentRoomLimit,
                c.OpeningHours.Open,
                c.OpeningHours.Close,
                c.Address.Street,
                c.Address.PostalCode,
                c.Address.City))
            .ToListAsync();
    }

    async Task<ClinicDTO?> IClinicQueries.GetByIdAsync(Guid Id)
    {
        return await db.Clinics
            .AsNoTracking()
            .Where(c => c.Id == Id)
            .Select(c => new ClinicDTO(
                c.Name,
                c.TreatmentRoomLimit,
                c.OpeningHours.Open,
                c.OpeningHours.Close,
                c.Address.Street,
                c.Address.PostalCode,
                c.Address.City))
            .FirstOrDefaultAsync();
    }
}
