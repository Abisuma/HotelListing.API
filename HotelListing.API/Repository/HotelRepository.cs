using HotelListing.API.Data;
using HotelListingAPI.Models;
using HotelListing.API.Repository.IRepository;
using AutoMapper;

namespace HotelListing.API.Repository
{
    public class HotelRepository : GenericRepository<Hotel>, IHotel
    {
        private AppDbContext _dbContext;
        public HotelRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext, mapper )
        {
            _dbContext = dbContext;
        }
    }
}