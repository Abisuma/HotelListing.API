using HotelListing.API.Data;
using HotelListingAPI.Models;
using HotelListing.API.Repository.IRepository;

namespace HotelListing.API.Repository
{
    public class HotelRepository : GenericRepository<Hotel>, IHotel
    {
        private AppDbContext _dbContext;
        public HotelRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}