using HotelListing.API.Data;
using HotelListingAPI.Models;
using HotelListing.API.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using static HotelListing.API.Repository.BookHotelRepository;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using HotelListingAPI.DTOs.BookHotels;

namespace HotelListing.API.Repository
{
    public class BookHotelRepository : GenericRepository<BookHotel>, IBookHotel
    {
         private AppDbContext _dbContext;
       private readonly IMapper _mapper;
        public BookHotelRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
         {
            _dbContext = dbContext;
            _mapper = mapper;   
         }

        //public async Task<BookHotelDTO> GetBookHotelDetails(int id)
        //{

        //    var BookHotel = _dbContext.BookHotels
        // .Include(u => u.Hotels)
        // .AsEnumerable() // Materialize the query before projection
        // .FirstOrDefault(u => u.BookHotelId == id);

        //    if (BookHotel == null)
        //    {
        //        return null; // Return null if the BookHotel is not found
        //    }

        //    return _mapper.Map<BookHotelDTO>(BookHotel);
        //}
    }
}