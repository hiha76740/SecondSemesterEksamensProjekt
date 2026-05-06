using BookRight.DomainLib.Enums;

namespace BookRight.DomainLib.Entities.Treatments.Physiotherapy;

public class Physiotherapy45 : Treatment
{
    public Physiotherapy45()
    {
        Name = "Physiotherapy";
        Price = 589m;
        Duration = TimeSpan.FromMinutes(45);
        CertificationRequired = CertificationTypes.Physiotherapy;
    }
}
