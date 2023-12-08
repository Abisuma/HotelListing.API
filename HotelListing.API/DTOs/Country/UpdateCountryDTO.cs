using HotelListing.API.Repository.IRepository;

namespace HotelListing.API.DTOs.Country
{
    public class UpdateCountryDTO : BaseCountryDto,IBaseDTO
    {
        public int Id { get; set; }//need this for the user to know the id of the countries for navigation purposes to other crud actions.

    }
}
