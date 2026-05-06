using BookRight.DomainLib.Entities.Clinics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookRight.InfrastructureLib.Persistance.EF_Configurations;

internal class ClinicConfiguration : IEntityTypeConfiguration<Clinic>
{
    public void Configure(EntityTypeBuilder<Clinic> builder)
    {
        throw new NotImplementedException();
    }
}
