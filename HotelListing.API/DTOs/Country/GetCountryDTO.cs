namespace HotelListing.API.DTOs.Country
{
    public class GetCountryDTO: BaseCountryDto
    {
        public int Id { get; set; }//need this for the user to know the id of the countries for navigation purposes to other crud actions.
        
    }
}
