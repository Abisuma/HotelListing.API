using System.ComponentModel.DataAnnotations.Schema;

namespace HotelListingAPI.Models
{
    public enum RoomTypes
    {
        Standard,Executive, Presidential
    }
    public class BookHotel
    {
        public RoomTypes RoomType { get; set; }
        private decimal _pricePerNight;
        public int Id { get; set; } 
        public string Description { get; set; }
        public decimal PricePerNight
        {
            get { return _pricePerNight; }
            set
            {
                _pricePerNight = value;
                GetRoomPriceBasedOnRoomType();
            }
        }
        public decimal PricePerThreeNight { get { return 0.8m * PricePerNight; } } 
        public decimal PricePerNightWeek { get { return 0.6m * PricePerNight; } } 
        
        public int CountryId { get; set; }
        [ForeignKey(nameof(CountryId))]
        public Country Country { get; set; }
        public int HotelId { get; set; }
        [ForeignKey(nameof(HotelId))]   
        public Hotel Hotel { get; set; }

        [ForeignKey(nameof(UserId))]
        public string UserId { get; set; }


        public void GetRoomPriceBasedOnRoomType()
        {
            
            if (RoomType == RoomTypes.Executive)
            {
                _pricePerNight = _pricePerNight * 2m;
            }
            else if (RoomType == RoomTypes.Presidential)
            {
                _pricePerNight = _pricePerNight * 3m;
            }
  
        }
        //}

    }
}
