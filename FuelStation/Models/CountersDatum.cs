using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OrganizationsWaterSupply.Models
{
    public partial class CountersDatum
    {
        [Key]
        public int CountersDataId { get; set; }
        public int? RegistrationNumber { get; set; }
        public DateTime? DataCheckDate { get; set; }
        public int? RateId { get; set; }
        public int? Volume { get; set; }

        public virtual RateOrg Rate { get; set; }
        public virtual Counter RegistrationNumberNavigation { get; set; }
    }
}
