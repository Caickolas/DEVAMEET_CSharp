using Microsoft.EntityFrameworkCore;

namespace DEVAMEET_CSharp.Models
{
    public class DevameetContext : DbContext
    {
        public DevameetContext(DbContextOptions<DevameetContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}
