using BookRight.DomainLib.Enums;

namespace BookRight.DomainLib.Entities.Treatments.DietaryGuidance;

public class DietaryGuidance30 : Treatment
{
    public DietaryGuidance30()
    {
        Name = "Dietary Guidance follow-up";
        Price = 450m;
        Duration = TimeSpan.FromMinutes(30);
        CertificationRequired = CertificationTypes.Dietary;
    }
}
