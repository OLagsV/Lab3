using OrganizationsWaterSupply.Models;
using System.Collections.Generic;

namespace OrganizationsWaterSupply.Services
{
    public interface IOrganizationsService
    {
        public IEnumerable<Organization> GetOrganizations(int rowsNumber = 20);
    }
}
