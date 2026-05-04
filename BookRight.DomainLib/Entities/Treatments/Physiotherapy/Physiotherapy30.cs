namespace BookRight.DomainLib.Entities.Treatments.Physiotherapy;

public class Physiotherapy30 : Treatment
{
    public Physiotherapy30()
    {
        Name = "Physiotherapy";
        Price = 395m;
        Duration = TimeSpan.FromMinutes(30);
    }
}
