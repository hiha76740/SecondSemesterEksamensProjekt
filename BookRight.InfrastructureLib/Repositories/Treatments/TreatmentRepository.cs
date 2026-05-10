using BookRight.ApplicationLib.Repositories;
using BookRight.DomainLib.Entities.Treatments;
using BookRight.InfrastructureLib.Persistance.DbContextFiles;
using Microsoft.EntityFrameworkCore;

namespace BookRight.InfrastructureLib.Repositories.Treatments;

public class TreatmentRepository(BookRightDbContext db) : ITreatmentRepository
{
    async Task<Treatment?> ITreatmentRepository.GetByIdAsync(Guid id)
    {
        return await db.Treatments
            .FirstOrDefaultAsync(t => t.Id == id);
    }
}
