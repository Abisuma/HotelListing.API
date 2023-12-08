using HotelListingAPI.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelListingAPI.DTOs.BookHotels
{
    public class BaseBookHotelDTO
    {
        public string Description { get; set; }
        public decimal PricePerNight { get; set; }
        public RoomTypes RoomType { get; set; }
        [ForeignKey(nameof(CountryId))]
        public int CountryId { get; set; }
        //public Country Country { get; set; }
        [ForeignKey(nameof(HotelId))]
        public int HotelId { get; set; }
        //public Hotel Hotel { get; set; }

        [ForeignKey(nameof(UserId))]
        public string UserId { get; set; }

    }
}
