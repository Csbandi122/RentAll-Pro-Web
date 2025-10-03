using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentAll_Pro_web.Data.Models
{
    public class FinancialDevice
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int FinancialId { get; set; }
        [ForeignKey("FinancialId")]
        public Financial Financial { get; set; }

        [Required]
        public int DeviceId { get; set; }
        [ForeignKey("DeviceId")]
        public Device Device { get; set; }
    }
}