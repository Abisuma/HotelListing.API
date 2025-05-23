﻿using HotelListing.API.DTOs.Country;
using HotelListingAPI.Models;

namespace HotelListing.API.Repository.IRepository
{
    public interface ICountry: IGenericRepository<Country>
    {
        public Task<CountryDTO> GetCountryDetails(int id);
    }
}
