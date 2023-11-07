using HotelListing.API.DTOs.Hotel;

namespace HotelListing.API.DTOs.Country
{
    public class CountryDTO : BaseCountryDto
    {
        public int Id { get; set; }
        public List<HotelDTO> Hotels { get; set; }

    }


}
