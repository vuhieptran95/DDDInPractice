using System;
using DDDInPractice.Domains;
using Microsoft.EntityFrameworkCore;

namespace DDDInPractice.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
            
        }
        
        public DbSet<VendingMachine> VendingMachines { get; set; }
        public DbSet<Slot> Slots { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VendingMachine>(e =>
            {
                e.HasKey(m => m.Id);
                e.Property(m => m.Id).HasColumnName("MachineId");
                e.Property(m => m.IsInTransaction).HasColumnName("IsInTransaction");
                e.HasMany(m => m.Slots).WithOne(s => s.VendingMachine).HasForeignKey(s => s.VendingMachineId);
                e.OwnsOne(m => m.MachineMoney, money =>
                {
                    money.Property(p => p.Five).HasColumnName("AmountOf5K");
                    money.Property(p => p.Ten).HasColumnName("AmountOf10K");
                    money.Property(p => p.Twenty).HasColumnName("AmountOf20K");
                    money.Property(p => p.Fifty).HasColumnName("AmountOf50K");
                    money.Property(p => p.OneHundred).HasColumnName("AmountOf100K");
                    money.Property(p => p.TwoHundred).HasColumnName("AmountOf200K");
                    money.Property(p => p.FiveHundred).HasColumnName("AmountOf500K");
                });

                e.Ignore(m => m.DomainEvents);
                e.Ignore(m => m.SelectedSlots);
                e.Ignore(m => m.InitialCustomerMoney);
                e.Ignore(m => m.CurrentAmountCustomerMoney);
            });

            modelBuilder.Entity<Slot>(e =>
            {
                e.HasKey(s => s.Id);
                e.Property(s => s.Id).HasColumnName("SlotId");
                e.Property(s => s.Position).HasColumnName("Position");
                e.Property(s => s.Price).HasColumnName("Price");
                e.Property(s => s.ProductCount).HasColumnName("ProductCount");
                e.Property(s => s.ProductName).HasColumnName("ProductName");
                e.Property(s => s.VendingMachineId).HasColumnName("VendingMachineId");
                e.HasOne(s => s.VendingMachine).WithMany(m => m.Slots).HasForeignKey(s => s.VendingMachineId);
            });

        }
    }
}