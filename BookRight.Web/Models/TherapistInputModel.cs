using System.ComponentModel.DataAnnotations;

namespace BookRight.Web.Models;

public class TherapistInputModel
{
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; } = null!;

    [Required]
    public string AuthorizationNumber { get; set; }
        = null!;

    [Required]
    [Range(1, 999999999)]
    public decimal HourlyRate { get; set; }

    [Required]
    public string EmailAddress { get; set; }
        = null!;

    [Required]
    public string PhoneNumber { get; set; }
        = null!;

    [Required]
    public string Street { get; set; }
        = null!;

    [Required]
    public string PostalCode { get; set; }
        = null!;

    [Required]
    public string City { get; set; }
        = null!;

    public List<string> Certifications { get; set; }
        = new();

    public List<Guid> AssociatedClinicIds { get; set; }
        = new();
}