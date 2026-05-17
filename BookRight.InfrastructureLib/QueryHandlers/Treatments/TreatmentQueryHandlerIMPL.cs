using BookRight.FacadeLib.DTO;
using BookRight.FacadeLib.Queries.Treatments;
using BookRight.InfrastructureLib.Persistance.DbContextFiles;
using Microsoft.EntityFrameworkCore;

namespace BookRight.InfrastructureLib.QueryHandlers.Treatments;

public class TreatmentQueryHandlerIMPL(BookRightDbContext db) : ITreatmentQueries
{
    async Task<IReadOnlyList<TreatmentDTO>> ITreatmentQueries.GetAllAsync()
    {
        return await db.Treatments
            .AsNoTracking()
            .Select(t => new TreatmentDTO(
                t.Id,
                t.Name,
                t.Price,
                t.Duration,
                t.CertificationRequired.ToString(),
                t.MaxParticipants))
            .ToListAsync();
    }

    async Task<TreatmentDTO?> ITreatmentQueries.GetByIdAsync(Guid Id)
    {
        return await db.Treatments
            .AsNoTracking()
            .Where(t => t.Id == Id)
            .Select(t => new TreatmentDTO(
                t.Id,
                t.Name,
                t.Price,
                t.Duration,
                t.CertificationRequired.ToString(),
                t.MaxParticipants))
            .FirstOrDefaultAsync();
    }
}
