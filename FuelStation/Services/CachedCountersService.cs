using OrganizationsWaterSupply.Data;
using OrganizationsWaterSupply.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace OrganizationsWaterSupply.Services
{
    public class CachedCountersService : ICachedCountersService
    {
        private readonly OrganizationsWaterSupplyContext _dbContext;
        private readonly IMemoryCache _memoryCache;

        public CachedCountersService(OrganizationsWaterSupplyContext dbContext, IMemoryCache memoryCache)
        {
            _dbContext = dbContext;
            _memoryCache = memoryCache;
        }
        public IEnumerable<Counter> GetCounters(int rowsNumber = 20)
        {
            return _dbContext.Counters.Include(x => x.Model).Include(x => x.Organization).Take(rowsNumber).ToList();
        }

        public void AddCounters(string cacheKey, int rowsNumber = 20)
        {
            IEnumerable<Counter> Counters = _dbContext.Counters.Include(x => x.Model).Include(x => x.Organization).Take(rowsNumber).ToList();
            if (Counters != null)
            {
                _memoryCache.Set(cacheKey, Counters, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(270)
                });

            }

        }
        public IEnumerable<Counter> GetCounters(string cacheKey, int rowsNumber = 20)
        {
            IEnumerable<Counter> Counters;
            if (!_memoryCache.TryGetValue(cacheKey, out Counters))
            {
                Counters = _dbContext.Counters.Include(x => x.Model).Include(x => x.Organization).Take(rowsNumber).ToList();
                if (Counters != null)
                {
                    _memoryCache.Set(cacheKey, Counters,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(270)));
                }
            }
            return Counters;
        }

        public IEnumerable<CounterModel> GetCounterModels(int rowsNumber = 20)
        {
            return _dbContext.CounterModels.Take(rowsNumber).ToList();
        }
        public IEnumerable<Counter> GetCountersByOrganization(string orgName)
        {
            return _dbContext.Counters.Include(x => x.Organization).Include(x => x.Model).Where(x => x.Organization.OrgName == orgName).ToList();
        }
        public IEnumerable<Counter> GetCountersByRegNumAndModel(int regNum, int modelId)
        {
            return _dbContext.Counters.Include(x => x.Organization).Include(x => x.Model).Where(x => x.RegistrationNumber == regNum && x.ModelId == modelId).ToList();
        }
    }
}

