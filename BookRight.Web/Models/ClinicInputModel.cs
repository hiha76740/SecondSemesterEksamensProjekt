using System.ComponentModel.DataAnnotations;

namespace BookRight.Web.Models
{
    public class ClinicInputModel
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        [Range(1, 999999999)]
        public int TreatmentRoomLimit { get; set; } = 1;
        [Required]
        public string Street { get; set; } = null!;
        [Required]
        public string PostalCode { get; set; } = null!;
        [Required]

        public string City { get; set; } = null!;
        [Required]

        public List<OpeningHourInputModel> OpeningHours { get; set; } = new();


    }
}
