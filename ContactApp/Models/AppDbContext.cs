using Microsoft.EntityFrameworkCore;

namespace ContactApp.Models
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
           : base(options)
        {
        }

        public DbSet<Contact> Contacts { get; set; }
    }
}
