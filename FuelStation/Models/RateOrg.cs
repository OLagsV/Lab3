using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OrganizationsWaterSupply.Models
{
    public partial class RateOrg
    {
        public RateOrg()
        {
            CountersData = new HashSet<CountersDatum>();
        }
        [Key]
        public int RateId { get; set; }
        public int? OrganizationId { get; set; }

        public virtual Organization Organization { get; set; }
        public virtual Rate Rate { get; set; }
        public virtual ICollection<CountersDatum> CountersData { get; set; }
    }
}
