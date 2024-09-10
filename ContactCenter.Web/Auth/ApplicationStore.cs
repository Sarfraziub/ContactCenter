using ContactCenter.Data;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Abstractions;

namespace ContactCenter.Web.Auth
{
    public class ApplicationStoreContext : DbContext

    {
        public ApplicationStoreContext(DbContextOptions<ApplicationStoreContext> options)
    : base(options)
        {
        }

        public ApplicationStoreContext() { }
        public DbSet<OpenIddict.EntityFrameworkCore.Models.OpenIddictEntityFrameworkCoreApplication> OpenIddictEntityFrameworkCoreApplications { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseInMemoryDatabase(databaseName: "Db");
            }

        }

    }
}
