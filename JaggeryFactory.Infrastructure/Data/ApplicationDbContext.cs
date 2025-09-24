using JaggeryAgro.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace JaggeryAgro.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        // ✅ DbSets
        public DbSet<LaborTypeRate> LaborTypeRates { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Deposit> Deposits { get; set; }
        public DbSet<Labor> Labors { get; set; }
        public DbSet<LaborType> LaborTypes { get; set; }        
        public DbSet<WeeklyPayment> WeeklyPayments { get; set; }
        public DbSet<AdvancePayment> AdvancePayments { get; set; }
        public DbSet<WeeklySalary> WeeklySalaries { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<JaggeryProduce> JaggeryProduces { get; set; }
        public DbSet<Farmer> Farmers { get; set; }
        public DbSet<CanePurchase> CanePurchases { get; set; }
        public DbSet<CaneAdvance> CaneAdvances { get; set; }
        public DbSet<CanePayment> CanePayments { get; set; }
        public DbSet<LaborPayment> LaborPayments { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<ExpenseType> ExpenseTypes { get; set; }
        public DbSet<Dealer> Dealers { get; set; }
        public DbSet<JaggerySale> JaggerySales { get; set; }
        public DbSet<DealerAdvance> DealerAdvances { get; set; }
        public DbSet<SplitwisePayment> SplitwisePayments { get; set; }
       

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ✅ Apply decimal precision globally
            foreach (var property in modelBuilder.Model
                .GetEntityTypes()
                .SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetPrecision(18);
                property.SetScale(2);
            }

            // ✅ Ignore MVC UI types
            modelBuilder.Ignore<SelectListItem>();

            // ✅ Relationships
            modelBuilder.Entity<Labor>()
                .HasOne(l => l.LaborType)
                .WithMany()
                .HasForeignKey(l => l.LaborTypeId);

            modelBuilder.Entity<LaborType>()
                .Property(x => x.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Farmer>()
                .HasIndex(f => new { f.Name, f.Mobile })
                .IsUnique(false);

            modelBuilder.Entity<CanePurchase>()
                .HasOne(cp => cp.Farmer)
                .WithMany(f => f.CanePurchases)
                .HasForeignKey(cp => cp.FarmerId)
                .OnDelete(DeleteBehavior.Restrict);

            // CaneAdvance precision
            modelBuilder.Entity<CaneAdvance>()
                .Property(a => a.RemainingAmount).HasPrecision(18, 2);
            modelBuilder.Entity<CaneAdvance>()
                .Property(a => a.Amount).HasPrecision(18, 2);

            // CanePayment precision
            modelBuilder.Entity<CanePayment>()
                .Property(p => p.GrossAmount).HasPrecision(18, 2);
            modelBuilder.Entity<CanePayment>()
                .Property(p => p.AdvanceAdjusted).HasPrecision(18, 2);
            modelBuilder.Entity<CanePayment>()
                .Property(p => p.NetAmount).HasPrecision(18, 2);
            modelBuilder.Entity<CanePayment>()
                .Property(p => p.CarryForwardAdvance).HasPrecision(18, 2);

            // Expense relationships
            modelBuilder.Entity<Expense>()
                .HasOne(e => e.PaidBy)
                .WithMany()
                .HasForeignKey(e => e.PaidById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Expense>()
                .HasOne(e => e.ExpenseType)
                .WithMany()
                .HasForeignKey(e => e.ExpenseTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Expense>()
                .HasOne(e => e.SplitwisePayment)
                .WithMany()
                .HasForeignKey(e => e.SplitwisePaymentId)
                .OnDelete(DeleteBehavior.NoAction);

            // SplitwisePayment relationships
            modelBuilder.Entity<SplitwisePayment>()
                .HasOne(p => p.FromMember)
                .WithMany()
                .HasForeignKey(p => p.FromMemberId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<SplitwisePayment>()
                .HasOne(p => p.ToMember)
                .WithMany()
                .HasForeignKey(p => p.ToMemberId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
