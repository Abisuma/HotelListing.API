using HotelListing.API.DTOs.User;
using Microsoft.AspNetCore.Identity;

namespace HotelListing.API.Repository.IRepository
{
    public interface IAuthManager
    {
        Task<IEnumerable<IdentityError>> Register(APIUserDto apiUserDto);
        Task<AuthResponseDto> Login(LoginDto loginuserdto);

    }
}
