namespace HotelListing.API.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICountry Country { get; }
        IHotel Hotel { get; }
    }
}
