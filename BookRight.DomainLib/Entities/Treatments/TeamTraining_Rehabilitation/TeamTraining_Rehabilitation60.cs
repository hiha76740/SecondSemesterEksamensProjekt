using BookRight.DomainLib.Enums;

namespace BookRight.DomainLib.Entities.Treatments.TeamTraining_Rehabilitation;

public class TeamTraining_Rehabilitation60 : Treatment
{
    public int TeamSizeLimit { get; private set; }

    public TeamTraining_Rehabilitation60()
    {
        Name = "Team Training Rehabilitation";
        Price = 150;
        Duration = TimeSpan.FromMinutes(60);
        TeamSizeLimit = 6;
        CertificationRequired = CertificationTypes.TeamTraining;
    }
}
