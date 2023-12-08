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
using HotelListingAPI.DTOs.BookHotels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace HotelListingAPI.Controllers
{
    [Route("api/BookHotels")]
    [ApiController]
    public class BookHotelsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public BookHotelsController(IUnitOfWork unitOfWork)
        {
             _unitOfWork = unitOfWork;  
        }

        // POST: api/BookHotels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<BookHotelDTO>> PostBookHotel([FromBody]BaseBookHotelDTO bookHotelDto)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return BadRequest("User ID not provided or invalid");
            }

            bookHotelDto.UserId = userId;

            var bookHotel = await _unitOfWork.BookHotel.AddAsync<BaseBookHotelDTO, BookHotelDTO>(bookHotelDto);

            return CreatedAtAction("GetBookHotel", new { id = bookHotel.HotelId }, bookHotel);
        }

        //// GET: api/BookHotels
        [HttpGet]
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult<IEnumerable<BookHotelDTO>>> GetBookHotel(int id)
        {
             var bookHotel = await _unitOfWork.BookHotel.GetAsync<BookHotelDTO>(id);
            return Ok(bookHotel);
        }

        //// GET: api/BookHotels/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<BookHotel>> GetBookHotel(int id)
        //{
        //    var bookHotel = await _context.BookHotel.FindAsync(id);

        //    if (bookHotel == null)
        //    {
        //        return NotFound();
        //    }

        //    return bookHotel;
        //}

        // PUT: api/BookHotels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutBookHotel(int id, BookHotel bookHotel)
        //{
        //    if (id != bookHotel.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(bookHotel).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!BookHotelExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}



        // DELETE: api/BookHotels/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteBookHotel(int id)
        //{
        //    var bookHotel = await _context.BookHotel.FindAsync(id);
        //    if (bookHotel == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.BookHotel.Remove(bookHotel);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool BookHotelExists(int id)
        //{
        //    return _context.BookHotel.Any(e => e.Id == id);
        //}
    }
}
