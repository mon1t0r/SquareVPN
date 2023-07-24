using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
    [Table("device")]
    [PrimaryKey(nameof(UUID))]
    public class Device
    {
        [Column("device_uuid")]
        public Guid UUID { get; set; }
        [Required, Column("user_uuid")]
        public Guid UserUUID { get; set; }
        [Column("ipv4_address"), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? IPV4Address { get; set; }
        [Required, Column("name")]
        public string Name { get; set; }
        [Column("created"), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedTimeStamp { get; set; }
        [Required, Column("public_key")]
        public string? PublicKey { get; set; }
        [Required, Column("refresh_token")]
        public string? RefreshToken { get; set; }
    }
}
