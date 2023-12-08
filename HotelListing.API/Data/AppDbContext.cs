using HotelListing.API.Data.Configurations;
using HotelListingAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Serilog.Sinks.SystemConsole.Themes;

namespace HotelListing.API.Data
{
    public class AppDbContext : IdentityDbContext<APIUser > 
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Hotel>Hotels { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<BookHotel> BookHotels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<BookHotel>()
          .HasOne(bh => bh.Country)
          .WithMany()
          .HasForeignKey(bh => bh.CountryId)
          .OnDelete(DeleteBehavior.Restrict); // or DeleteBehavior.NoAction

            modelBuilder.Entity<BookHotel>()
                .HasOne(bh => bh.Hotel)
                .WithMany()
                .HasForeignKey(bh => bh.HotelId)
                .OnDelete(DeleteBehavior.Restrict);
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
           
        }

    }
}
