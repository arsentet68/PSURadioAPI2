using Microsoft.EntityFrameworkCore;
using PSURadioAPI2.Models;

namespace PSURadioAPI2.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Friend> Friends { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}