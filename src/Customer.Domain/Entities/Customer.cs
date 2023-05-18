using System.ComponentModel.DataAnnotations;

namespace Customer.Domain.Entities
{
    public class Customers
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; } = string.Empty;

        [Range(0, 100, ErrorMessage = "Age must be between 0 and 100")]
        public int? Age { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; } = string.Empty;
    }
}