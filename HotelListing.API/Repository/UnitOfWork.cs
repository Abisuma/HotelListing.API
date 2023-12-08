using AutoMapper;
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
        public IBookHotel BookHotel { get; private set; }   

        public UnitOfWork(AppDbContext appDbContext, IMapper mapper )
        {
            _appDbContext = appDbContext;
            Country = new CountryRepository(_appDbContext, mapper); 
            Hotel = new HotelRepository(_appDbContext, mapper); 
            BookHotel = new BookHotelRepository(_appDbContext, mapper);
        }
    }
}
