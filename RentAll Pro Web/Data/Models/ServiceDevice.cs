using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RentAll_Pro_web.Data.Models; // <-- Ezt a sort is ellenőrizzük

namespace RentAll_Pro_web.Data.Models
{
    public class ServiceDevice
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ServiceId { get; set; }
        [ForeignKey("ServiceId")]
        public Service Service { get; set; }

        [Required]
        public int DeviceId { get; set; }
        [ForeignKey("DeviceId")]
        public Device Device { get; set; }
    }
}