using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
    [Table("User")]
    public class User
    {
        [Required, Key, Column("user_id")]
        public ulong Id { get; set; }
        [Required, Column("last_payment")]
        public ulong LastPaymentTimeStamp { get; set; }
    }
}
