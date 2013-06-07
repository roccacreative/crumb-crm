using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CrumbCRM.Web.Models
{
    public class LeadViewModel
    {
        public List<Lead> Leads { get; set; }

        public int TotalProspecting { get; set; }
        public int TotalQualified { get; set; }
        public int TotalUnqualified { get; set; }
        public int TotalQuote { get; set; }
        public int TotalClosure { get; set; }
        public int TotalWon { get; set; }
        public int TotalLost { get; set; }
        public int TotalNew { get; set; }
        public int TotalEmailed { get; set; }
        public int TotalNoAnswer { get; set; }
        public int TotalNotInterested { get; set; }
        public int TotalCallback { get; set; }
        public int TotalDoNotContact { get; set; }

        public LeadType? LeadType { get; set; }
        public PriorityType? PriorityType { get; set; }    
    }
}