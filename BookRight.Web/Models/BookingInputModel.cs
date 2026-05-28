using System.ComponentModel.DataAnnotations;

namespace BookRight.Web.Models;

public class BookingInputModel
{
    public Guid Id { get; set; }
    public string Status { get; set; } = string.Empty;
    public List<Guid> Participants { get; set; } = [];
    public string DiscountTypeUsed { get; set; } = string.Empty;


    [Required]
    public DateTime Start { get; set; }
    [Required]
    public DateTime End { get; set; }

    [Required]
    public Guid TreatmentId { get; set; }

    [Required]
    public Guid TherapistId { get; set; }

    [Required]
    public Guid ClinicId { get; set; }

    [Required]
    public decimal Price { get; set; }

    [Required]
    public int ParticipantLimit { get; set; }


}