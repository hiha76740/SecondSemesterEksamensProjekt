using System.ComponentModel.DataAnnotations;

namespace BookRight.Web.Models
{
    public class OpeningHourInputModel
    {
        [Required]
        public string WeekDay { get; set; } = null!;
        public TimeOnly? OpeningTime { get; set; }
        public TimeOnly? ClosingTime { get; set; }
        public bool IsClosed { get; set; }
    }
}
