using Microsoft.EntityFrameworkCore;
using SharedCard.Api.Models;

namespace SharedCard.Api.Data
{
    public class SharedCardDBContext : DbContext
    {
        public SharedCardDBContext(DbContextOptions<SharedCardDBContext> options)
        : base(options)
        {
        }
        public DbSet<Person> People => Set<Person>();
        public DbSet<Bill> Bills => Set<Bill>();
        public DbSet<BillingCycle> BillingCycles => Set<BillingCycle>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>(entity =>
            {
                entity.ToTable("People");
                entity.HasKey(e => e.PersonId);
            });

            modelBuilder.Entity<Bill>(entity =>
            {
                entity.ToTable("Bills");
                entity.HasKey(e => e.BillId);

                entity.HasOne(d => d.Person)
                      .WithMany(p => p.Bills)
                      .HasForeignKey(d => d.PersonId);

                entity.HasOne(d => d.BillingCycle)
                      .WithMany(c => c.Bills)
                      .HasForeignKey(d => d.CycleId);
            });

            modelBuilder.Entity<BillingCycle>(entity =>
            {
                entity.ToTable("BillingCycles");
                entity.HasKey(e => e.CycleId);
            });
        }
    }
}
