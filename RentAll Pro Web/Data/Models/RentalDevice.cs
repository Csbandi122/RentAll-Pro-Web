using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentAll_Pro_web.Data.Models
{
    public class RentalDevice
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int RentalId { get; set; }
        [ForeignKey("RentalId")]
        public Rental Rental { get; set; }

        [Required]
        public int DeviceId { get; set; }
        [ForeignKey("DeviceId")]
        public Device Device { get; set; }
    }
}