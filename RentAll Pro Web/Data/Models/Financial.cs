using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentAll_Pro_Web.Data.Models
{
    public class Financial
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string TicketNr { get; set; }

        [Required]
        public string EntryType { get; set; }

        [Required]
        public string SourceType { get; set; }

        public int? SourceId { get; set; }

        public DateTime Date { get; set; }

        public string? Comment { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }

        public ICollection<FinancialDevice> FinancialDevices { get; set; } = new List<FinancialDevice>();
    }
}