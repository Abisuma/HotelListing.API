using HotelListing.API.Data;
using HotelListingAPI.Models;
using HotelListing.API.Repository.IRepository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using HotelListingAPI.DTOs;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Elfie.Serialization;

namespace HotelListing.API.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {

        private readonly AppDbContext db;
        private readonly IMapper _mapper;
        

        public GenericRepository(AppDbContext dbContext, IMapper mapper)
        {
            db = dbContext;
            _mapper = mapper;   
        }
        


        public async Task<TResult> AddAsync<TSource, TResult>(TSource source)
        {
            var entity = _mapper.Map<T>(source);

            await db.Set<T>().AddAsync(entity);
            await db.SaveChangesAsync();

            return _mapper.Map<TResult>(entity);
        }
        

        public async Task<List<TResult>> GetAllAsync<TResult>()
        {
            return await db.Set<T>().ProjectTo<TResult>(_mapper.ConfigurationProvider).ToListAsync();   
        }

        public async Task<PagedResult<TResult>> GetAllAsync<TResult>(QueryParameter queryParameter)
        {
            var totalsize = await db.Set<T>().CountAsync();
            var items = await db.Set<T>()
                .Skip(queryParameter.StartIndex).Take(queryParameter.PageSize)
                .ProjectTo<TResult>(_mapper.ConfigurationProvider)
                .ToListAsync(); 

            return new PagedResult<TResult> 
            { 
                Items = items,
                TotalCount = totalsize,
                PageNumber = queryParameter.PageNumber,
                RecordNumber = queryParameter.PageSize
            
            };
        }

        public async Task<TResult> GetAsync<TResult>(int? id)
        {
            if (id == 0 || id == null)
            {
                return default;
            }
            var item = await db.Set<T>().FindAsync(id);
            if (item == null)
            {
                return default;
            }
            var itemResult = _mapper.Map<TResult>(item);

            return itemResult;
        }

        public async Task UpdateAsync<TSource>(int id, TSource source) where TSource : IBaseDTO
        {
            if (id != source.Id)
            {
                return;
            }

            var entity = await GetAsync<T>(id);
            if (entity == null) { return; }

            _mapper.Map(source, entity);

            db.Set<T>().Update(entity);
            await db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int? id)
        {

            var entity = await GetAsync<T>(id);
            if (entity == null)
            {
                return;
            }

            db.Set<T>().Remove(entity);
            await db.SaveChangesAsync();
        }
        public async Task<bool> Exists(int id)
        {
            var entity = await GetAsync<T>(id);
            return entity != null;
        }


    }
}
