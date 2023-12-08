using HotelListing.API.DTOs.Country;
using HotelListingAPI.Models;

namespace HotelListing.API.Repository.IRepository
{
    public interface IBookHotel: IGenericRepository<BookHotel>
    {
        //public Task<CountryDTO> GetCountryDetails(int id);
    }
}
