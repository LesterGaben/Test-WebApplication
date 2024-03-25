using Microsoft.EntityFrameworkCore;
using TestWebApplication.Models;

namespace TestWebApplication.Context {
    public class ApplicationDbContext : DbContext {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) {
        }

        public DbSet<ConfigurationNode> ConfigurationNodes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ConfigurationNode>()
               .HasMany(n => n.Children)
               .WithOne(n => n.Parent)
               .HasForeignKey(n => n.ParentId)
               .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
