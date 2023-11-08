using OrganizationsWaterSupply.Models;
using System.Collections.Generic;

namespace OrganizationsWaterSupply.Services
{
    public interface ICachedCountersService
    {
        public IEnumerable<Counter> GetCounters(int rowsNumber = 20);
        public void AddCounters(string cacheKey, int rowsNumber = 20);
        public IEnumerable<Counter> GetCounters(string cacheKey, int rowsNumber = 20);
        public IEnumerable<CounterModel> GetCounterModels(int rowsNumber = 20);
        public IEnumerable<Counter> GetCountersByOrganization(string orgName);
        public IEnumerable<Counter> GetCountersByRegNumAndModel(int regNum, int modelId);
    }
}
