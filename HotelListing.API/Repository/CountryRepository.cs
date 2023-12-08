using HotelListing.API.Data;
using HotelListingAPI.Models;
using HotelListing.API.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using static HotelListing.API.Repository.CountryRepository;
using AutoMapper;
using HotelListing.API.DTOs.Country;
using AutoMapper.QueryableExtensions;

namespace HotelListing.API.Repository
{
    public class CountryRepository : GenericRepository<Country>, ICountry
    {
         private AppDbContext _dbContext;
       private readonly IMapper _mapper;
        public CountryRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
         {
            _dbContext = dbContext;
            _mapper = mapper;   
         }

        public async Task<CountryDTO> GetCountryDetails(int id)
        {

            var country = _dbContext.Countries
         .Include(u => u.Hotels)
         .AsEnumerable() // Materialize the query before projection
         .FirstOrDefault(u => u.CountryId == id);

            if (country == null)
            {
                return null; // Return null if the country is not found
            }

            return _mapper.Map<CountryDTO>(country);
        }
    }
}