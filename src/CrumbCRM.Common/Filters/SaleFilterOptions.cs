using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrumbCRM.Filters
{
    public class SaleFilterOptions
    {
        public SaleType? Status { get; set; }

        public string Order { get; set; }

        public List<object> Campaigns { get; set; }

        public List<object> Tags { get; set; }

        public bool IsQualified { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string SearchTerm { get; set; }
    }
}
