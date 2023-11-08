using System;
using System.Collections.Generic;
using OrganizationsWaterSupply.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace OrganizationsWaterSupply.Data
{
    public partial class OrganizationsWaterSupplyContext : DbContext
    {
        public OrganizationsWaterSupplyContext()
        {
        }

        public OrganizationsWaterSupplyContext(DbContextOptions<OrganizationsWaterSupplyContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Counter> Counters { get; set; }
        public virtual DbSet<CounterModel> CounterModels { get; set; }
        public virtual DbSet<CountersCheck> CountersChecks { get; set; }
        public virtual DbSet<CountersDatum> CountersData { get; set; }
        public virtual DbSet<Organization> Organizations { get; set; }
        public virtual DbSet<Rate> Rates { get; set; }
        public virtual DbSet<RateOrg> RateOrgs { get; set; }

        
    }
}
