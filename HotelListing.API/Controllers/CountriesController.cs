using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelListing.API.Data;
using HotelListingAPI.Models;
using AutoMapper;
using HotelListing.API.DTOs.Country;
using HotelListing.API.Repository.IRepository;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Authorization;

namespace HotelListing.API.Controllers
{
    [Route("api/Countries")]
    [ApiController]
    [Authorize]
    
    public class CountriesController : ControllerBase
    {
        
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;   

        public CountriesController( IMapper mapper, IUnitOfWork unitOfWork)
        {
            
            _mapper = mapper;
            _unitOfWork = unitOfWork;   
        }

        // POST: api/Countries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        
        public async Task<ActionResult<Country>> PostCountry(CreateCountryDTO countryDTO)
        {
           //mapping dto to model
            var country = _mapper.Map<Country>(countryDTO);

            await _unitOfWork.Country.AddAsync(country);
            return CreatedAtAction("GetCountry", new { id = country.CountryId }, country);
        }

        // GET: api/Countries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetCountryDTO>>> GetCountries()
        {
            var countries = await _unitOfWork.Country.GetAllAsync();
            var records = _mapper.Map<List<GetCountryDTO>>(countries);//mapped the retrieved Country query result to the GetCountryDTO 
            return Ok(records);
        }

        // GET: api/Countries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CountryDTO>> GetCountry(int id)
        {
            var countrywithhotels = await _unitOfWork.Country.GetCountryDetails(id); 

            var countryDtoWithHotels = _mapper.Map<CountryDTO>(countrywithhotels);

            if (countryDtoWithHotels == null)
            {
                return NotFound();
            }

            return countryDtoWithHotels;
        }

        // PUT: api/Countries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCountry(int id, UpdateCountryDTO updateCountryDto)
        {
            if (id != updateCountryDto.Id)
            {
                return BadRequest();
            }

            //_context.Entry(country).State = EntityState.Modified;
            var country = await _unitOfWork.Country.GetAsync(id);       
            if (country == null) 
            {
                return NotFound();
            }
            
           _mapper.Map(updateCountryDto, country);//assigned the values from the updateCountryDto to Country.country and EF will track and modified the values in the database.
           
            try
            {
                await _unitOfWork.Country.UpdateAsync(country);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (! await CountryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        

        // DELETE: api/Countries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            
            await _unitOfWork.Country.DeleteAsync(id);

            return NoContent();
        }

        private async Task<bool> CountryExists(int id)
        {
            return await _unitOfWork.Country.Exists(id);
        }
    }
}
