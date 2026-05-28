using System.ComponentModel.DataAnnotations;

namespace BookRight.Web.Models
{
    public class CustomerInputModel
    {
        public Guid Id { get; set; }

        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public DateOnly BirthDate { get; set; } = DateOnly.FromDateTime(DateTime.Today);

        public string Note { get; set; } = string.Empty;

        [Required]
        public string Street { get; set; } = string.Empty;

        [Required]
        public string PostalCode { get; set; } = string.Empty;

        [Required]
        public string City { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; } = string.Empty;

        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;

        public Guid? PreferredTherapist { get; set; }
    }
}
