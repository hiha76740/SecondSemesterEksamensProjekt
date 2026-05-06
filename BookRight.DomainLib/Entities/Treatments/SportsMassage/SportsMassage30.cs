using BookRight.DomainLib.Enums;

namespace BookRight.DomainLib.Entities.Treatments.SportsMassage;

public class SportsMassage30 : Treatment
{
    public SportsMassage30()
    {
        Name = "Sports Massage";
        Price = 350m;
        Duration = TimeSpan.FromMinutes(30);
        CertificationRequired = CertificationTypes.Message;
    }
}
