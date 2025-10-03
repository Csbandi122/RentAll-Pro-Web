using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentAll_Pro_web.Data.Models
{
    public class Service
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string TicketNr { get; set; }

        [Required]
        public string ServiceType { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Technician { get; set; }

        public DateTime ServiceDate { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal CostAmount { get; set; }

        public ICollection<ServiceDevice> ServiceDevices { get; set; } = new List<ServiceDevice>();
    }
}