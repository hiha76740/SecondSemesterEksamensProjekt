using BookRight.ApplicationLib.Repositories;
using BookRight.DomainLib.Entities.Clinics;
using BookRight.InfrastructureLib.Persistance.DbContextFiles;
using Microsoft.EntityFrameworkCore;

namespace BookRight.InfrastructureLib.Repositories.Clinics;

public class ClinicRepository(BookRightDbContext db) : IClinicRepository
{
    async Task IClinicRepository.AddAsync(Clinic clinic)
    {
        await db.AddAsync(clinic);
    }

    async Task<Clinic?> IClinicRepository.GetByIdAsync(Guid id)
    {
        return await db.Clinics
            .Include(c => c.OpeningHours)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    async Task IClinicRepository.SaveAsync()
    {
        await db.SaveChangesAsync();
    }
}
