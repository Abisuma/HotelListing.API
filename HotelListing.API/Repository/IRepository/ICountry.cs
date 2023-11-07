using HotelListingAPI.Models;

namespace HotelListing.API.Repository.IRepository
{
    public interface ICountry: IGenericRepository<Country>
    {
        public Task<Country> GetCountryDetails(int? id);
    }
}
