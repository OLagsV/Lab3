using System;
using System.Collections.Generic;

namespace OrganizationsWaterSupply.Models
{
    public partial class CountersCheck
    {
        public int CountersCheckId { get; set; }
        public int? RegistrationNumber { get; set; }
        public DateTime? CheckDate { get; set; }
        public string CheckResult { get; set; }

        public virtual Counter RegistrationNumberNavigation { get; set; }
    }
}
