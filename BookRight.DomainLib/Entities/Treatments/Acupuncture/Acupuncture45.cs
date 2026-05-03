namespace BookRight.DomainLib.Entities.Treatments.Acupuncture;

public class Acupuncture45 : Treatment
{
    public Acupuncture45()
    {
        Name = "Acupuncture";
        Price = 550m;
        Duration = TimeSpan.FromMinutes(45);
    }
}
