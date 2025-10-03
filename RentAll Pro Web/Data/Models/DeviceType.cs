using System.ComponentModel.DataAnnotations;

namespace RentAll_Pro_web.Data.Models
{
    public class DeviceType
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string TypeName { get; set; }

        public ICollection<Device> Devices { get; set; } = new List<Device>();
    }
}