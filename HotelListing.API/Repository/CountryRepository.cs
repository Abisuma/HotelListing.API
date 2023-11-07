using HotelListing.API.Data;
using HotelListingAPI.Models;
using HotelListing.API.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using static HotelListing.API.Repository.CountryRepository;

namespace HotelListing.API.Repository
{
    public class CountryRepository : GenericRepository<Country>, ICountry
    {
         private AppDbContext _dbContext;
         public CountryRepository(AppDbContext dbContext) : base(dbContext)
         {
            _dbContext = dbContext;
         }

        public async Task<Country> GetCountryDetails(int? id)
        {
            if (id == null) return null;
            return  _dbContext.Countries.Include(u => u.Hotels).FirstOrDefault(u => u.CountryId == id);
        }
    }
}