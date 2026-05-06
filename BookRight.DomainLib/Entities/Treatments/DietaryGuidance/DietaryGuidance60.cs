using BookRight.DomainLib.Enums;

namespace BookRight.DomainLib.Entities.Treatments.DietaryGuidance;

public class DietaryGuidance60 : Treatment
{
    public DietaryGuidance60()
    {
        Name = "Dietary Guidance first time";
        Price = 799m;
        Duration = TimeSpan.FromMinutes(60);
        CertificationRequired = CertificationTypes.Dietary;
    }
}
