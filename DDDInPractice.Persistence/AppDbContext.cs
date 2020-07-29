using System;
using DDDInPractice.Domains;
using Microsoft.EntityFrameworkCore;

namespace DDDInPractice.Persistence
{
    public class AppDbContext : DbContext
    {
        public DbSet<VendingMachine> VendingMachines { get; set; }
        public DbSet<Slot> Slots { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VendingMachine>(e =>
            {
                e.HasKey(m => m.Id);
                e.HasMany(m => m.Slots).WithOne(s => s.VendingMachine).HasForeignKey(s => s.VendingMachineId);
            });
        }
    }
}