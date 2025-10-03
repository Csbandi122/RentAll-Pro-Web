using Microsoft.EntityFrameworkCore;
using RentAll_Pro_Web.Data.Models;

namespace RentAll_Pro_Web.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<DeviceType> DeviceTypes { get; set; }
        public DbSet<Financial> Financials { get; set; }
        public DbSet<FinancialDevice> FinancialDevices { get; set; }
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<RentalDevice> RentalDevices { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<ServiceDevice> ServiceDevices { get; set; }
        public DbSet<Setting> Settings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}