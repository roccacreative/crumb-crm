using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CrumbCRM.Web.Models
{
    public class SearchViewModel
    {
        public List<Lead> Leads { get; set; }
        public List<Sale> Sales { get; set; }
        public List<Contact> Contacts { get; set; }
       
    }
}