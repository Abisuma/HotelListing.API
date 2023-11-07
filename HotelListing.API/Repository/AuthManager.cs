using AutoMapper;
using HotelListing.API.DTOs.User;
using HotelListingAPI.Models;
using HotelListing.API.Repository.IRepository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HotelListing.API.Repository
{
    public class AuthManager : IAuthManager
    {
        private readonly IMapper _mapper;
        private readonly UserManager<APIUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        public AuthManager(IMapper mapper, UserManager<APIUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _mapper = mapper; 
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration; 
        }

        

        public async Task<IEnumerable<IdentityError>> Register(APIUserDto apiUserDto)
        {
            //get the DTO and map it
            var user = _mapper.Map<APIUser>(apiUserDto);
            //making the user name as the email of the dto 
            user.UserName = apiUserDto.Email;

            //create user
            var result = await _userManager.CreateAsync(user, apiUserDto.Password);

            //if user created successfully

            if (result.Succeeded)
            {

                var roleExist = await _roleManager.RoleExistsAsync("User");

                if (!roleExist)
                {
                    var roleResult = await _roleManager.CreateAsync(new IdentityRole("User"));

                    if (!roleResult.Succeeded)
                    {
                        return result.Errors;
                    }
                }

                await _userManager.AddToRoleAsync(user, "User");
            }
            //else
            return  result.Errors;
        }

        public async Task<AuthResponseDto> Login(LoginDto loginuserdto)
        {
            //validate the user and hi/her password. 
            var user = await  _userManager.FindByEmailAsync(loginuserdto.Email);
            bool isValidUser =await _userManager.CheckPasswordAsync(user, loginuserdto.Password);

            if (user == null || isValidUser == false) 
            {
                return null;
            }

            var token = await GenerateToken(user);

            return new AuthResponseDto
            {
                Token = token,
                UserId = user.Id,

            };
            
         }

        private async Task<string> GenerateToken(APIUser apiUser)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //get the roles of the user

            var roles = await _userManager.GetRolesAsync(apiUser);
            var rolesClaim = roles.Select(x => new Claim(ClaimTypes.Role,x)).ToList();

            var userClaims = await _userManager.GetClaimsAsync(apiUser);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, apiUser.Email), //sub is the person to which the jwt has been issued to.
                new Claim(JwtRegisteredClaimNames.Jti, apiUser.Email),//Jti generate new guids to prevent playback
                new Claim(JwtRegisteredClaimNames.Email, apiUser.Email),
                new Claim("uid", apiUser.Id),
            }.Union(userClaims).Union(rolesClaim);

            var token = new JwtSecurityToken
                (
                    issuer: _configuration["JwtSettings:Issuer"],
                    audience: _configuration["JwtSettings:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddDays(Convert.ToInt32(_configuration["JwtSettings:DurationInDays"])),
                    signingCredentials: credentials
                ) ;
            return new JwtSecurityTokenHandler().WriteToken(token);//return a newly created jwt token.
        }
    }
}
