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
        private readonly ILogger<AuthManager> _logger;


        private APIUser _user;//global user field  
        private const string loginProvider = "HotelListingAPI";
        private const string refreshToken =  "RefreshToken";
        private IdentityResult result;
        private static string adminDynamicPassword;
        public AuthManager(IMapper mapper, UserManager<APIUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, ILogger<AuthManager> logger)
        {
            _mapper = mapper; 
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _logger = logger;
        }



        public async Task<IEnumerable<IdentityError>> Register(APIUserDto apiUserDto)
        {
            try
            {
                //get the DTO and map it
                _user = _mapper.Map<APIUser>(apiUserDto);
                //making the user name as the email of the dto 
                _user.UserName = apiUserDto.Email;

                //Cheking if to registration is for admin.
                if (apiUserDto.Email.EndsWith("@superadmin.com"))
                {
                    // Ensure the email matches the admin format
                    // Define the predetermined password for admin
                    adminDynamicPassword = "SuperAdmin@" + DateTime.Now.ToString("MM") + (DateTime.Now.Day + 7).ToString();

                    // Check if the provided password matches the predetermined admin password
                    if (apiUserDto.Password != adminDynamicPassword)
                    {
                        // Return an error if the password doesn't match
                        return new List<IdentityError>{
                         new IdentityError { Code = "InvalidPassword", Description = "Invalid password for admin registration and you are not authorize  as an admin." }};
                    }


                    var resultAdmin = await _userManager.CreateAsync(_user, apiUserDto.Password);

                    if (resultAdmin.Succeeded)
                    {
                        // Check if the 'Admin' role exists
                        var adminRoleExists = await _roleManager.RoleExistsAsync("Admin");
                        if (!adminRoleExists)
                        {
                            // If the 'Admin' role doesn't exist, create it
                            var adminRoleResult = await _roleManager.CreateAsync(new IdentityRole("Admin"));

                            if (!adminRoleResult.Succeeded)
                            {
                                return adminRoleResult.Errors;
                            }
                        }

                        // Add the user to the 'Admin' role
                        await _userManager.AddToRoleAsync(_user, "Admin");
                    }



                }
                else
                {
                    // If the email format does not match, proceed with regular user registration logic
                    result = await _userManager.CreateAsync(_user, apiUserDto.Password);

                    ////if user created successfully

                    if (result.Succeeded)
                    {

                        var roleexist = await _roleManager.RoleExistsAsync("user");

                        if (!roleexist)
                        {
                            var roleresult = await _roleManager.CreateAsync(new IdentityRole("user"));

                            if (!roleresult.Succeeded)
                            {
                                return result.Errors;
                            }
                        }

                        await _userManager.AddToRoleAsync(_user, "user");
                    }


                }
                return result?.Errors;
            }
            catch (Exception ex)
            {
                // Log the exception details
                _logger.LogError(ex, "An error occurred during user registration.");

                // Rethrow the exception to propagate it further if needed
                throw;
            }
            
        } 
        public async Task<AuthResponseDto> Login(LoginDto loginuserdto)
        {
            //validate the user and his/her password. 
            _user = await  _userManager.FindByEmailAsync(loginuserdto.Email);

            //if (_user.Email.EndsWith()  ) {  
            
            
            //}    
           
            bool isValidUser = await _userManager.CheckPasswordAsync(_user, loginuserdto.Password);

            if (_user == null || isValidUser == false) 
            {
                return null;
            }

            var token = await GenerateToken();

            return new AuthResponseDto
            {
                Token = token,
                UserId = _user.Id,
                RefreshToken = await CreateRefreshToken()

            };


        }

        private async Task<string> GenerateToken()
        {
            //var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
            //var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //

            //var roles = await _userManager.GetRolesAsync(_user);
            //var rolesClaim = roles.Select(x => new Claim(ClaimTypes.Role,x)).ToList();

            //var userClaims = await _userManager.GetClaimsAsync(_user);

            //var claims = new List<Claim>
            //{
            //    new Claim(JwtRegisteredClaimNames.Sub, _user.Email),
            //    new Claim(JwtRegisteredClaimNames.Jti, _user.Email),//Jti generate new guids to prevent playback
            //    new Claim(JwtRegisteredClaimNames.Email, _user.Email),
            //    new Claim("uid", _user.Id),
            //}.Union(userClaims).Union(rolesClaim);

            //var token = new JwtSecurityToken
            //    (
            //        issuer: _configuration["JwtSettings:Issuer"],
            //        audience: _configuration["JwtSettings:Audience"],
            //        claims: claims,
            //        expires: DateTime.Now.AddDays(Convert.ToInt32(_configuration["JwtSettings:DurationInDays"])),
            //        signingCredentials: credentials
            //    ) ;
            //return new JwtSecurityTokenHandler().WriteToken(token);

            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));

            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);

            //get the roles of the user

            var roles = await _userManager.GetRolesAsync(_user);
            var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x)).ToList();
            var userClaims = await _userManager.GetClaimsAsync(_user);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, _user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, _user.Email),
                new Claim("uid", _user.Id),
            }
            .Union(userClaims).Union(roleClaims);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(Convert.ToInt32(_configuration["JwtSettings:DurationInDays"])),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);//return a newly created jwt token.
        }

        public async Task<string> CreateRefreshToken()
        {
            await _userManager.RemoveAuthenticationTokenAsync(_user,loginProvider,refreshToken);//discard the former token
            var newRefreshToken = await _userManager.GenerateUserTokenAsync(_user, loginProvider, refreshToken);//generate the new token
            var result = await _userManager.SetAuthenticationTokenAsync(_user, loginProvider, refreshToken, newRefreshToken); //set the new token

            return newRefreshToken;
        }

        public async Task<AuthResponseDto> VerifyRefreshToken(AuthResponseDto result)
        {
           var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
           var tokenContent = jwtSecurityTokenHandler.ReadJwtToken(result.Token);
            var username = tokenContent.Claims.ToList().FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email)?.Value;

            _user = await _userManager.FindByNameAsync(username);
            if (_user == null  || _user.Id != result.UserId)
            {
                return null;
            }

            var validateusertoken = await _userManager.VerifyUserTokenAsync(_user, loginProvider, refreshToken, result.RefreshToken);

            if (validateusertoken)
            {
                var token = await GenerateToken();

                return new AuthResponseDto
                {
                    Token = token,
                    UserId = _user.Id,
                    RefreshToken = await CreateRefreshToken()

                };


            }
             
            await _userManager.UpdateSecurityStampAsync(_user);
            return null;
        }
    }
}
 