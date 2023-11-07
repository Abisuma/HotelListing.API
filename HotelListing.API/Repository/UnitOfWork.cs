using HotelListing.API.Data;
using HotelListing.API.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.API.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private AppDbContext _appDbContext;
        public ICountry Country { get; private set; }
        
        public IHotel Hotel { get; private set; }   

        public UnitOfWork(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
            Country = new CountryRepository(_appDbContext); 
            Hotel = new HotelRepository(_appDbContext);
        }
    }
}
