using HotelListing.API.Data;
using HotelListingAPI.Models;
using HotelListing.API.Repository.IRepository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace HotelListing.API.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {

        private readonly AppDbContext db;
        

        public GenericRepository(AppDbContext dbContext)
        {
            db = dbContext;
        }
        public async Task<T> AddAsync(T entity)
        {
           await db.AddAsync(entity);
            await db.SaveChangesAsync();
            return entity;  
            
        }

        public Task<T> CreateAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await db.Set<T>().ToListAsync();  
        }
        
    
        public async Task<T> GetAsync(int? id)
        {
            if (id == null) return null;
            return await db.Set<T>().FindAsync(id);
        }

        public async Task UpdateAsync(T entity)
        {
            db.Set<T>().Update(entity);
            await db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int? id)
        {
            
            var entity = await GetAsync(id);
            if (entity == null)
            {
                return; 
            }
            
            db.Set<T>().Remove(entity);   
            await db.SaveChangesAsync();
        }
        public async Task<bool> Exists(int id)
        {
            var entity = await GetAsync(id); 
            return entity != null;  
        }
    }
}
