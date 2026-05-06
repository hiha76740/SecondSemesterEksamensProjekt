using BookRight.DomainLib.Enums;

namespace BookRight.DomainLib.Entities.Treatments.SportsMassage;

public class SportsMassage60 : Treatment
{
    public SportsMassage60()
    {
        Name = "Sports Massage";
        Price = 699m;
        Duration = TimeSpan.FromMinutes(60);
        CertificationRequired = CertificationTypes.Message;
    }
}

