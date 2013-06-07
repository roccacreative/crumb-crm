using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrumbCRM.Filters
{
    public class TaskFilterOptions
    {
        public string Order { get; set; }

        public bool? HasDeadline { get; set; }

        public bool? FutureOnly { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}
