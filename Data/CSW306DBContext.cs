using Microsoft.EntityFrameworkCore;
using CSW305Proj.Models;

namespace CSW305Proj.Data
{
    public class CSW306DBContext : DbContext
    {
       public CSW306DBContext(DbContextOptions options) : base (options) { 
       }
        public DbSet <Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<BikeCategory> BikeCategories { get; set; }
        public DbSet<Carousels> Carousels { get; set; }

        public DbSet<BikeStation> BikeStations { get; set; }
        public DbSet<Bike> Bikes { get; set; }
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<Notifications> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Bike>()
                .HasOne(b => b.BikeStation)
                .WithMany(s => s.Bikes)
                .HasForeignKey(b => b.BikeStationId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Bike>()
                .HasOne(b => b.BikeCategory)
                .WithMany(c => c.Bikes)
                .HasForeignKey(b => b.BikeCategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PaymentRental>()
            .HasKey(pr => new { pr.PaymentId, pr.RentalId });

            modelBuilder.Entity<PaymentRental>()
                .HasOne(pr => pr.Payment)
                .WithMany(p => p.PaymentRentals)
                .HasForeignKey(pr => pr.PaymentId);

            modelBuilder.Entity<PaymentRental>()
                .HasOne(pr => pr.Rental)
                .WithMany(r => r.PaymentRentals)
                .HasForeignKey(pr => pr.RentalId);

            modelBuilder.Entity<Rental>()
                .HasOne(r => r.Bike)
                .WithMany()
                .HasForeignKey(r => r.BikeId);

            modelBuilder.Entity<Rental>()
                .HasOne(r => r.User)
                .WithMany(u => u.Rentals)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserRole>()
                  .HasKey(ur => new { ur.UserId, ur.RoleId });

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);

            modelBuilder.Entity<Notifications>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);



        }


        }
}
