using JaggeryAgroManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace JaggeryAgroManagementSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<Labor> Labors { get; set; }
        public DbSet<LaborAttendance> LaborAttendances { get; set; }
        public DbSet<LaborPayment> LaborPayments { get; set; }
        public DbSet<Deposit> Deposits { get; set; }

        // Existing DbSet
        public DbSet<User> Users { get; set; }

    }

}
