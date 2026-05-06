using BookRight.DomainLib.Enums;

namespace BookRight.DomainLib.Entities.Treatments.Physiotherapy;

public class Physiotherapy60 : Treatment
{
    public Physiotherapy60()
    {
        Name = "Physiotherapy";
        Price = 745;
        Duration = TimeSpan.FromMinutes(60);
        CertificationRequired = CertificationTypes.Physiotherapy;
    }
}
