using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CrumbCRM.Web.Models
{
    public class HomeViewModel
    {
        public int TotalActiveDeals { get; set; }
        public decimal TotalSalesPipeline { get; set; }
        public int Last30Won { get; set; }
        public int Last30Lost { get; set; }
        public int Last30Unqualified { get; set; }
        public LeadType? LeadType { get; set; }
        public List<Task> Tasks { get; set; }

        public int Prev30Sales { get; set; }
        public int Prev30Leads { get; set; }
        public int Last30Sales { get; set; }
        public int Last30Leads { get; set; }

        public int Last30TotalProspecting { get; set; }
        public int Last30TotalQualified { get; set; }
        public int Last30TotalUnqualified { get; set; }
        public int Last30TotalQuote { get; set; }
        public int Last30TotalClosure { get; set; }
        public int Last30TotalWon { get; set; }
        public int Last30TotalLost { get; set; }
        public int Last30TotalNew { get; set; }
        public int Last30TotalEmailed { get; set; }
        public int Last30TotalNoAnswer { get; set; }
        public int Last30TotalNotInterested { get; set; }
        public int Last30TotalCallback { get; set; }
        public int Last30TotalDoNotContact { get; set; }

        public List<Activity> Activities { get; set; }
    }
}