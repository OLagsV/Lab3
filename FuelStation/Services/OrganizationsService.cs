using OrganizationsWaterSupply.Data;
using OrganizationsWaterSupply.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace OrganizationsWaterSupply.Services
{
    public class OrganizationsService : IOrganizationsService
    {
        private readonly OrganizationsWaterSupplyContext _dbContext;
        private readonly IMemoryCache _memoryCache;

        public OrganizationsService(OrganizationsWaterSupplyContext dbContext, IMemoryCache memoryCache)
        {
            _dbContext = dbContext;
            _memoryCache = memoryCache;
        }
        public IEnumerable<Organization> GetOrganizations(int rowsNumber = 20)
        {
            return _dbContext.Organizations.Take(rowsNumber).ToList();
        }

      

    }
}

