using System.ComponentModel.DataAnnotations;

namespace HotelListing.API.DTOs.User
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(15, ErrorMessage = "Your password is limited to {1} to {2} characters", MinimumLength = 6)]
        public string Password { get; set; }
    }
}
