using Microsoft.AspNetCore.Identity;

namespace HotelListingAPI.Models
{
    public class APIUser:IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
