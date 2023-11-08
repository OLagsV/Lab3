using System;
using System.Collections.Generic;

namespace OrganizationsWaterSupply.Models
{
    public partial class Rate
    {
        public int RateId { get; set; }
        public string RateName { get; set; }
        public decimal? Price { get; set; }

        public virtual RateOrg RateOrg { get; set; }
    }
}
