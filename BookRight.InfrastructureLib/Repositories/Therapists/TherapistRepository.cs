using BookRight.ApplicationLib.Repositories;
using BookRight.DomainLib.Entities.Therapists;
using BookRight.InfrastructureLib.Persistance.DbContextFiles;
using Microsoft.EntityFrameworkCore;

namespace BookRight.InfrastructureLib.Repositories.Therapists;

public class TherapistRepository(BookRightDbContext db) : ITherapistRepository
{
    async Task ITherapistRepository.AddAsync(Therapist therapist)
    {
        await db.AddAsync(therapist);
    }

    async Task<Therapist?> ITherapistRepository.GetByIdAsync(Guid id)
    {
        return await db.Therapists.FirstOrDefaultAsync(t => t.Id == id);
    }

    async Task ITherapistRepository.SaveAsync()
    {
        await db.SaveChangesAsync();
    }
}

