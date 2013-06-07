using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrumbCRM.Filters
{
    public class LeadFilterOptions
    {
        public LeadType? Status { get; set; }

        public string Order { get; set; }

        public List<object> Campaigns { get; set; }

        public List<object> Tags { get; set; }
    }
}
