using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
    [Table("Device")]
    [PrimaryKey(nameof(UserId), nameof(PublicKey))]
    public class Device
    {
        [Required, Column("user_id")]
        public ulong UserId { get; set; }
        [Required, Column("public_key")]
        public string? PublicKey { get; set; }
        [Required, Column("name")]
        public string? Name { get; set; }
    }
}
