using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
    [Table("User")]
    public class User
    {
        [Key, Column("user_uuid"), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid UUID { get; set; }
        [Column("user_id"), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public ulong Id { get; set; }
        [Column("paid_until"), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime PaidUntilTimeStamp { get; set; }
    }
}
