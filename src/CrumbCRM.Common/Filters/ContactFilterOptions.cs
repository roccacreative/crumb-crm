using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrumbCRM.Filters
{
    public class ContactFilterOptions
    {
        public ContactType? Type { get; set; }

        public string Order { get; set; }
    }
}
