using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OrganizationsWaterSupply.Models
{
    public partial class Counter
    {
        public Counter()
        {
            CountersChecks = new HashSet<CountersCheck>();
            CountersData = new HashSet<CountersDatum>();
        }
        [Key]
        public int RegistrationNumber { get; set; }
        public int? ModelId { get; set; }
        public DateTime? TimeOfInstallation { get; set; }
        public int? OrganizationId { get; set; }

        public virtual CounterModel Model { get; set; }
        public virtual Organization Organization { get; set; }
        public virtual ICollection<CountersCheck> CountersChecks { get; set; }
        public virtual ICollection<CountersDatum> CountersData { get; set; }
    }
}
