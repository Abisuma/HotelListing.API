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
using HotelListingAPI.DTOs;

namespace HotelListing.API.Controllers
{
    [Route("api/Countries")]
    [ApiController]


    public class CountriesController : ControllerBase
    {

        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CountriesController> _logger;

        public CountriesController(IMapper mapper, IUnitOfWork unitOfWork, ILogger<CountriesController> logger)
        {

            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        // POST: api/Countries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Produces("application/json")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Country>> PostCountry(CreateCountryDTO countryDTO)
        {
            var country = await _unitOfWork.Country.AddAsync<CreateCountryDTO, Country>(countryDTO);
            return CreatedAtAction("GetCountry", new { id = country.CountryId }, country);
        }

        // GET: api/Countries
        [HttpGet("GetAll")]
        [Produces("application/json")]
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult<IEnumerable<GetCountryDTO>>> GetCountries()
        {
            var countries = await _unitOfWork.Country.GetAllAsync<GetCountryDTO>();
            return Ok(countries);
        }

        // GET: api/Countries/?StartIndex=0&PageSize=25&PageNumber=1
        //[HttpGet]
        //[Produces("application/json")]
        //[Authorize(Roles = "Admin, User")]
        //public async Task<ActionResult<PagedResult<GetCountryDTO>>> GetPagedCountries([FromQuery] QueryParameter queryParameter)
        //{
        //    var pagedCountriesResult = await _unitOfWork.Country.GetAllAsync<GetCountryDTO>(queryParameter);//mapping done in the genericrepository method.
            
        //    return Ok(pagedCountriesResult);
        //}

        // GET: api/Countries/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult<CountryDTO>> GetCountry(int id)
        {
            var countrywithhotels = await _unitOfWork.Country.GetCountryDetails(id); 
            return Ok(countrywithhotels);   
        }
        
        [HttpGet("GetOneCountry/{id}")]
        [Authorize(Roles = "Admin, User")]
       
        public async Task<ActionResult<CountryDTO>> GetOneCountry(int id)
        {
            var countrywithhotels = await _unitOfWork.Country.GetAsync<CountryDTO>(id); 

            if (countrywithhotels == null)
            {
                _logger.LogWarning($"something went wrong in {nameof(GetOneCountry)},country with the id: {id} does not exist");
                return NotFound();
            }

            return countrywithhotels;
        }

        // PUT: api/Countries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutCountry(int id, UpdateCountryDTO updateCountryDto)
        {
            
            try
            {
                await _unitOfWork.Country.UpdateAsync(id, updateCountryDto);
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
        [Authorize(Roles ="Admin")]
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
