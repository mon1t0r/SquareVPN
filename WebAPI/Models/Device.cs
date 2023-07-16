using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
    [Table("Device")]
    [PrimaryKey(nameof(UserUUID), nameof(UUID))]
    public class Device
    {
        [Required, Column("user_uuid")]
        public Guid UserUUID { get; set; }
        [Required, Column("device_uuid")]
        public Guid UUID { get; set; }
        [Required, Column("ipv4_address")]
        public string IPV4Address { get; set; }
        [Required, Column("name")]
        public string Name { get; set; }
        [Required, Column("created")]
        public DateTime CreatedTimeStamp { get; set; }
        [Required, Column("public_key")]
        public string PublicKey { get; set; }
    }
}
