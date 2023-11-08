using System;
using System.Collections.Generic;

namespace OrganizationsWaterSupply.Models
{
    public partial class Organization
    {
        public Organization()
        {
            Counters = new HashSet<Counter>();
            RateOrgs = new HashSet<RateOrg>();
        }

        public int OrganizationId { get; set; }
        public string OrgName { get; set; }
        public string OwnershipType { get; set; }
        public string Adress { get; set; }
        public string DirectorFullname { get; set; }
        public string DirectorPhone { get; set; }
        public string ResponsibleFullname { get; set; }
        public string ResponsiblePhone { get; set; }

        public virtual ICollection<Counter> Counters { get; set; }
        public virtual ICollection<RateOrg> RateOrgs { get; set; }
    }
}
