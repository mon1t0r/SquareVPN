using Microsoft.EntityFrameworkCore;

namespace WebAPI.Models
{
    public class WebContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Device> Devices { get; set; }

        public WebContext(DbContextOptions<WebContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
