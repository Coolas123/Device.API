using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public sealed class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions dbOptions):base(dbOptions) {
            
        }

        public DbSet<Device> Devices { get; set; } = null!;

        //protected override void OnModelCreating(ModelBuilder modelBuilder) {
        //    modelBuilder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);
        //}
    }
}
