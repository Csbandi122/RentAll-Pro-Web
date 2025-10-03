using System.ComponentModel.DataAnnotations;

namespace RentAll_Pro_Web.Data.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Zipcode { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string IdNumber { get; set; }

        public string? Comment { get; set; }

        public ICollection<Rental> Rentals { get; set; } = new List<Rental>();
    }
}