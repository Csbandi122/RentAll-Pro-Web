using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentAll_Pro_web.Data.Models
{
    public class Device
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string DeviceName { get; set; }

        [Required]
        public int DeviceTypeId { get; set; }
        [ForeignKey("DeviceTypeId")]
        public DeviceType DeviceType { get; set; }

        [Required]
        public string Serial { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal RentPrice { get; set; }

        public bool Available { get; set; } = true;

        public string? Picture { get; set; }

        public int RentCount { get; set; } = 0;

        public string? Notes { get; set; }

        public ICollection<RentalDevice> RentalDevices { get; set; } = new List<RentalDevice>();
        public ICollection<ServiceDevice> ServiceDevices { get; set; } = new List<ServiceDevice>();
        public ICollection<FinancialDevice> FinancialDevices { get; set; } = new List<FinancialDevice>();
    }
}