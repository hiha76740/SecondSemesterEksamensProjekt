using BookRight.DomainLib.Entities.Treatments;
using BookRight.DomainLib.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookRight.InfrastructureLib.Persistance.EF_Configurations;

internal class TreatmentConfiguration : IEntityTypeConfiguration<Treatment>
{
    public void Configure(EntityTypeBuilder<Treatment> builder)
    {
        builder.Property(t => t.CertificationRequired)
           .HasConversion<string>();

        builder.HasData(
            new { Id = Guid.Parse("68a14b26-15d6-4831-a876-221b01260c6b"), Name = "Acupuncture 45", Price = 550m, Duration = TimeSpan.FromMinutes(45), CertificationRequired = CertificationTypes.Acupuncture, MaxParticipants = 1 },
            new { Id = Guid.Parse("f748fc78-c660-4de0-a1a8-f7c0b0930f98"), Name = "Dietary Guidance follow-up", Price = 450m, Duration = TimeSpan.FromMinutes(30), CertificationRequired = CertificationTypes.Dietary, MaxParticipants = 1 },
            new { Id = Guid.Parse("79b16d43-7475-4409-bd12-ddd0e2067863"), Name = "Dietary Guidance first time", Price = 799m, Duration = TimeSpan.FromMinutes(60), CertificationRequired = CertificationTypes.Dietary, MaxParticipants = 1 },
            new { Id = Guid.Parse("2f1196bb-8d78-444c-bcdb-992806905c6e"), Name = "Physiotherapy 30", Price = 395m, Duration = TimeSpan.FromMinutes(30), CertificationRequired = CertificationTypes.Physiotherapy, MaxParticipants = 1 },
            new { Id = Guid.Parse("53bb0fec-b7ee-45c7-ae0e-0db7f508658d"), Name = "Physiotherapy 45", Price = 589m, Duration = TimeSpan.FromMinutes(45), CertificationRequired = CertificationTypes.Physiotherapy, MaxParticipants = 1 },
            new { Id = Guid.Parse("4ced2638-f883-42fa-ad82-24902b3b1ba4"), Name = "Physiotherapy 60", Price = 745m, Duration = TimeSpan.FromMinutes(60), CertificationRequired = CertificationTypes.Physiotherapy, MaxParticipants = 1 },
            new { Id = Guid.Parse("b83d128e-f408-40f8-b4ad-25c31250cf2b"), Name = "Sports Massage 30", Price = 350m, Duration = TimeSpan.FromMinutes(30), CertificationRequired = CertificationTypes.Massage, MaxParticipants = 1 },
            new { Id = Guid.Parse("710cfebd-245c-410d-b63f-9e066f77de39"), Name = "Sports Massage 60", Price = 699m, Duration = TimeSpan.FromMinutes(60), CertificationRequired = CertificationTypes.Massage, MaxParticipants = 1 },
            new { Id = Guid.Parse("4401a516-76ba-497f-beba-aee749850ddd"), Name = "Team Training Rehabilitation", Price = 150m, Duration = TimeSpan.FromMinutes(60), CertificationRequired = CertificationTypes.Rehabilitation, MaxParticipants = 6 }
            );
    }
}
