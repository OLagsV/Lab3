using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OrganizationsWaterSupply.Models
{
    public partial class CounterModel
    {
        public CounterModel()
        {
            Counters = new HashSet<Counter>();
        }
        [Key]
        public int ModelId { get; set; }
        public string ModelName { get; set; }
        public string Manufacturer { get; set; }
        public int? ServiceTime { get; set; }

        public virtual ICollection<Counter> Counters { get; set; }
    }
}
