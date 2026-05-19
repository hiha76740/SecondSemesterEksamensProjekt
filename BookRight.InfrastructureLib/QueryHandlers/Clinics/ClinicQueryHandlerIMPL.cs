using BookRight.FacadeLib.DTO;
using BookRight.FacadeLib.Queries.Clinics;
using BookRight.InfrastructureLib.Persistance.DbContextFiles;
using Microsoft.EntityFrameworkCore;

namespace BookRight.InfrastructureLib.QueryHandlers.Clinics;

public class ClinicQueryHandlerIMPL(BookRightDbContext db) : IClinicQueries
{
    async Task<IReadOnlyList<ClinicDTO>> IClinicQueries.GetAllAsync()
    {
        var clinics = await db.Clinics
            .AsNoTracking()
            .Include(oh => oh.OpeningHours)
            .ToListAsync();


        return clinics.Select(c => new ClinicDTO(
         c.Id,
         c.Name,
         c.TreatmentRoomLimit,
         c.OpeningHours
         .OrderBy(oh => oh.WeekDay)
         .Select(oh => new OpeningHourDTO(oh.WeekDay.ToString(), oh.OpeningTime, oh.ClosingTime, oh.IsClosed)).ToList(),
         c.Address.Street,
         c.Address.PostalCode,
         c.Address.City))
         .ToList();
    }

    async Task<ClinicDTO?> IClinicQueries.GetByIdAsync(Guid Id)
    {
        var clinic = await db.Clinics
            .AsNoTracking()
            .Include(oh => oh.OpeningHours)
            .Where(c => c.Id == Id)
            .ToListAsync();

        return clinic.Select(c => new ClinicDTO(
          c.Id,
          c.Name,
          c.TreatmentRoomLimit,
          c.OpeningHours
          .OrderBy(oh => oh.WeekDay)
          .Select(oh => new OpeningHourDTO(oh.WeekDay.ToString(), oh.OpeningTime, oh.ClosingTime, oh.IsClosed))
          .ToList(),
          c.Address.Street,
          c.Address.PostalCode,
          c.Address.City))
           .FirstOrDefault();
    }
}
