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
        public async Task<ActionResult<Hotel>> PostHotel(CreateHotelDto createHotelDto)
        {
            var hotel =_mapper.Map<Hotel>(createHotelDto);

            await _unitOfWork.Hotel.AddAsync(hotel);

            return CreatedAtAction("GetHotel", new { id = hotel.Id }, hotel);
        }

        // GET: api/Hotels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Hotel>>> GetHotels()
        {
            var hotels = await _unitOfWork.Hotel.GetAllAsync();
            var records = _mapper.Map<List<HotelDTO>>(hotels); 
            return Ok(records); 
        }

        // GET: api/Hotels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GetHotelDTO>> GetHotel(int id)
        {
            var hotel = await _unitOfWork.Hotel.GetAsync(id);

            if (hotel == null)
            {
                return NotFound();
            }
            
            var recordhotel = _mapper.Map<GetHotelDTO>(hotel);
            return recordhotel;
        }

        // PUT: api/Hotels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHotel(int id, UpdateHotelDTO updateHotelDTO)
        {
            if (id != updateHotelDTO.Id)
            {
                return BadRequest();
            }
            var hotelToBeUpdated = await _unitOfWork.Hotel.GetAsync(id);
            if (hotelToBeUpdated == null)   return NotFound();
            _mapper.Map(updateHotelDTO,hotelToBeUpdated);

            try
            {
                await _unitOfWork.Hotel.UpdateAsync(hotelToBeUpdated);
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
