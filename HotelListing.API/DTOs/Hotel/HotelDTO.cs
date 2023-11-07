using System.ComponentModel.DataAnnotations.Schema;

namespace HotelListing.API.DTOs.Hotel
{
    public class HotelDTO : BaseHotelDto
    {
        public int Id { get; set; }
        
        //i can decide on whatever properties i would like to display to the user.
    }
}
