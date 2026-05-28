using System.ComponentModel.DataAnnotations;

namespace BookRight.Web.Models;

public class CampaignInputModel
{
    public Guid CampaignId { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    [Range(1, 100)]
    public decimal DiscountProcentage { get; set; } = 1m;

    [Required]
    public DateOnly CampaignStart { get; set; } = DateOnly.FromDateTime(DateTime.Today);

    [Required]
    public DateOnly CampaignEnd { get; set; } = DateOnly.FromDateTime(DateTime.Today.AddDays(7));

    public List<Guid> AssignedTreatments { get; set; } = [];

    public string Status { get; set; } = string.Empty;
}
