using BookRight.DomainLib.Entities.Therapists;

namespace BookRight.ApplicationLib.Repositories;

public interface ITherapistRepository
{
    Task<Therapist?> GetAsync(Guid id);
}
