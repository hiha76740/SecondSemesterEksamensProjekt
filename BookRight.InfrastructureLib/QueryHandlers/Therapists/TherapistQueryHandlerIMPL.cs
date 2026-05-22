using BookRight.FacadeLib.DTO;
using BookRight.FacadeLib.Queries.Therapists;
using BookRight.InfrastructureLib.Persistance.DbContextFiles;
using Microsoft.EntityFrameworkCore;
using static System.Net.WebRequestMethods;

namespace BookRight.InfrastructureLib.QueryHandlers.Therapists;

public class TherapistQueryHandlerIMPL(BookRightDbContext db) : ITherapistQueries
{
    async Task<IReadOnlyList<TherapistDTO>> ITherapistQueries.GetAllAsync()
    {
        var therapists = await db.Therapists
       .AsNoTracking()
       .ToListAsync();

        return therapists
        .Select(t => new TherapistDTO(
            t.Id,
            t.AuthorizationNumber,
            t.Name,
            t.HourlyRate,
            t.Address.Street,
            t.Address.PostalCode,
            t.Address.City,
            t.Email.EmailAddress,
            t.PhoneNumber.Number,
            t.CertificationTypes.Select(ct => ct.ToString()).ToList(),
            t.AssociatedClinics.ToList()
        ))
        .ToList();
    }

    async Task<TherapistDTO?> ITherapistQueries.GetByIdAsync(Guid Id)
    {
        var therapist = await db.Therapists
            .AsNoTracking()
            .Where(t => t.Id == Id)
            .ToListAsync();

        return therapist.Select(t => new TherapistDTO(
            t.Id,
            t.AuthorizationNumber,
            t.Name,
            t.HourlyRate,
            t.Address.Street,
            t.Address.PostalCode,
            t.Address.City,
            t.Email.EmailAddress,
            t.PhoneNumber.Number,
            t.CertificationTypes.Select(ct => ct.ToString()).ToList(),
            t.AssociatedClinics.ToList()
            ))
        .FirstOrDefault();
    }

    async Task<IReadOnlyList<TherapistDTO>> ITherapistQueries.SearchTherapistAsync(string filter)
    {
        var therapist = await db.Therapists
       .AsNoTracking()
       .Where(t =>
       t.Name.Contains(filter) ||
       t.Email.EmailAddress.Contains(filter) ||
       t.PhoneNumber.Number.Contains(filter))
       .ToListAsync();


        return therapist
            .Select(t => new TherapistDTO(
                t.Id,
                t.AuthorizationNumber,
                t.Name,
                t.HourlyRate,
                t.Address.Street,
                t.Address.PostalCode,
                t.Address.City,
                t.Email.EmailAddress,
                t.PhoneNumber.Number,
                t.CertificationTypes.Select(ct => ct.ToString()).ToList(),
                t.AssociatedClinics.ToList()
                ))
            .ToList();
    }
}
