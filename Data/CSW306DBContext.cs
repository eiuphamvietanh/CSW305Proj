using Microsoft.EntityFrameworkCore;
using CSW305Proj.Models;

namespace CSW305Proj.Data
{
    public class CSW306DBContext : DbContext
    {
       public CSW306DBContext(DbContextOptions options) : base (options) { 
       }
        public DbSet <Users> Users { get; set; }
        public DbSet<Customers> Customers { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Customers>()
                .HasOne(c => c.User)
                .WithOne(u => u.Customer)
                .HasForeignKey<Users>(u => u.CustomerId);  
        }
    }
}
