using System.ComponentModel.DataAnnotations;

namespace HotelListing.API.DTOs.User
{
    public class APIUserDto : LoginDto
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]  
        public string Address { get; set; }
        [Required]
        [Phone]
        [StringLength(11)]
        public string PhoneNumber { get; set; }
    }
}
