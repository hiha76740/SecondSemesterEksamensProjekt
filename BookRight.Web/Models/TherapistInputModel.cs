using System.ComponentModel.DataAnnotations;

namespace BookRight.Web.Models;

public class TherapistInputModel
{
    public Guid Id { get; set; }

    [Required]
    public string AuthorizationNumber { get; set; } = string.Empty;

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    [Range(1, 99999)]
    public decimal HourlyRate { get; set; }

    [Required]
    public string Street { get; set; } = string.Empty;

    [Required]
    public string PostalCode { get; set; } = string.Empty;

    [Required]
    public string City { get; set; } = string.Empty;

    [Required]
    public string EmailAddress { get; set; } = string.Empty;

    [Required]
    public string PhoneNumber { get; set; } = string.Empty;

    public List<string> Certifications { get; set; } = [];

    public List<Guid> AssociatedClinics { get; set; } = [];
}