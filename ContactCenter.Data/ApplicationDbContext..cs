using Microsoft.EntityFrameworkCore;

namespace ContactCenter.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<EdrsmUser> EdrsmUsers { get; set; }
        public DbSet<Country> Countries { get; set; } 
    }
}
