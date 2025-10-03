using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentAll_Pro_Web.Data.Models
{
    public class Rental
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string TicketNr { get; set; }

        [Required]
        public int CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }

        public DateTime RentStart { get; set; }

        public int RentalDays { get; set; }

        [Required]
        public string PaymentMode { get; set; }

        public string? Comment { get; set; }

        public string? Contract { get; set; }

        public string? Invoice { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalAmount { get; set; }

        public bool ReviewEmailSent { get; set; } = false;

        public bool ContractEmailSent { get; set; } = false;

        public bool InvoiceEmailSent { get; set; } = false;

        public ICollection<RentalDevice> RentalDevices { get; set; } = new List<RentalDevice>();
    }
}