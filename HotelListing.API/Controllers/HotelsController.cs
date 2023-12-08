using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelListing.API.Data;
using HotelListingAPI.Models;
using HotelListing.API.Repository.IRepository;
using AutoMapper;
using HotelListing.API.DTOs.Hotel;
using Microsoft.AspNetCore.Authorization;
using HotelListing.API.DTOs.Country;
using HotelListingAPI.DTOs;

namespace HotelListing.API.Controllers
{
    [Route("api/Hotels")]
    [ApiController]
    public class HotelsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public HotelsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // POST: api/Hotels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<HotelDTO>> PostHotel(CreateHotelDto createHotelDto)
        {
            var hotel = await _unitOfWork.Hotel.AddAsync<CreateHotelDto,HotelDTO>(createHotelDto);

            return CreatedAtAction("GetHotel", new { id = hotel.Id }, hotel);
        }

        // GET: api/Hotels
        [HttpGet("GetAllHotels")]
        [Authorize(Roles = "Admin, Users")]
        public async Task<ActionResult<IEnumerable<GetHotelDTO>>> GetHotels()
        {
            var hotels = await _unitOfWork.Hotel.GetAllAsync<GetHotelDTO>(); 
            return Ok(hotels);  
         }

        // GET: api/Countries/?StartIndex=0&PageSize=25&PageNumber=1
        //[HttpGet]
        //[Produces("application/json")]
        ////[Authorize(Roles = "Admin, Users")]
        //public async Task<ActionResult<PagedResult<GetHotelDTO>>> GetPagedHotels([FromQuery] QueryParameter queryParameter)
        //{
        //    var pagedHotelsResult = await _unitOfWork.Hotel.GetAllAsync<GetHotelDTO>(queryParameter);//mapping done in the genericrepository method.

        //    return Ok(pagedHotelsResult);
        //}

        // GET: api/Hotels/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, Users")]
        public async Task<ActionResult<GetHotelDTO>> GetHotel(int id)
        {
            var hotel = await _unitOfWork.Hotel.GetAsync<GetHotelDTO>(id);
            return Ok(hotel);
        }

        // PUT: api/Hotels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutHotel(int id, UpdateHotelDTO updateHotelDTO)
        {
           
            try
            {
                await _unitOfWork.Hotel.UpdateAsync(id,updateHotelDTO);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await HotelExists(id))
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

        

        // DELETE: api/Hotels/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            await _unitOfWork.Hotel.DeleteAsync(id);
            return NoContent();
        }

        private async Task<bool> HotelExists(int id)
        {
            return await _unitOfWork.Country.Exists(id);
        }
    }
}
