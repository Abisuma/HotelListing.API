using AutoMapper;
using HotelListing.API.DTOs.Country;
using HotelListing.API.DTOs.Hotel;
using HotelListing.API.DTOs.User;
using HotelListingAPI.DTOs.BookHotels;
using HotelListingAPI.Models;
using Microsoft.Build.Framework.Profiler;

namespace HotelListing.API.Configurations
{
    public class MapperConfig: Profile
    {
        public MapperConfig()
        {
                //Country mapping
                CreateMap<Country, CreateCountryDTO>().ReverseMap();
                CreateMap<Country, GetCountryDTO>().ReverseMap(); 
                CreateMap<Country, CountryDTO>().ReverseMap();
                CreateMap<Country, UpdateCountryDTO>().ReverseMap(); 

                //Hotel mapping

                CreateMap<Hotel, HotelDTO>().ReverseMap();
                CreateMap<Hotel, CreateHotelDto>().ReverseMap();
                CreateMap<Hotel, GetHotelDTO>().ReverseMap();
                CreateMap<Hotel, UpdateHotelDTO>().ReverseMap();

            //BookingHotel mapping

            CreateMap<BookHotel, BaseBookHotelDTO>().ReverseMap();
            CreateMap<BookHotel, BookHotelDTO>().ReverseMap();

            //user mapping
            CreateMap<APIUser, APIUserDto>().ReverseMap();
        }
    }
}
