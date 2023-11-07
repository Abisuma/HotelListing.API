namespace HotelListing.API.Repository.IRepository
{
    public interface IGenericRepository<T>where T: class
    {
        Task<T> CreateAsync(T entity);
        Task<T> GetAsync(int? id);
        Task UpdateAsync(T entity);  
        Task DeleteAsync(int? id);  
        Task<T> AddAsync(T entity);
        Task<List<T>> GetAllAsync();
        Task <bool> Exists(int id);    
    }
}
