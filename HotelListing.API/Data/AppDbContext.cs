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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.Entity<Country>().HasData(
                new Country
                {
                    CountryId = 1,
                    Name = "Nigeria",
                    ShortName = "NGA",

                },
                new Country
                {
                    CountryId = 2,
                    Name = "Mamlaka Saudiyya",
                    ShortName = "KSA",
                },
                new Country
                {
                    CountryId = 3,
                    Name = "United Arab Emirate",
                    ShortName = "UAE",

                }
                );

            modelBuilder.Entity<Hotel>().HasData(
               new Hotel
               {
                   Id = 1,
                   Name =  "Oriental Hotel",
                   Address= "Plot 445,Ozumba Mbadiwe,Victoria Island, Lagos.",
                   Rating = 4,
                   CountryId = 1,
                   
               },
               new Hotel
               {
                   Id = 2,
                   Name = "Hilton Riyadh Hotel",
                   Address = "6623 Al Shohadaa Eastern Ring Road, Riyadh.",
                   Rating = 5,
                   CountryId = 2,
                   
               },
               new Hotel
               {
                   Id = 3,
                   Name = "Swissôtel Al Murooj Dubai",
                   Address = "Al Mustaqbal St - opposite The Dubai Mall - Trade Centre - Dubai",
                   Rating = 5,
                   CountryId = 3,
                   
               }
               );
        }
    }
}
