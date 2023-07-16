using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
    [Table("User")]
    public class User
    {
        [Required, Key, Column("user_uuid")]
        public Guid UUID { get; set; }
        [Required, Column("user_id")]
        public ulong Id { get; set; }
        [Required, Column("paid_until")]
        public ulong PaidUntilTimeStamp { get; set; }
        [Column("refresh_token")]
        public string? RefreshToken { get; set; }
        [Required, Column("refresh_token_expiry")]
        public DateTime RefreshTokenExpiry { get; set; }
    }
}
