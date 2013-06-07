using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CrumbCRM.Web.Models
{
    public class SaleViewModel
    {
        public List<Sale> Sales { get; set; }

        public int TotalProspecting { get; set; }
        public int TotalQualified { get; set; }
        public int TotalUnqualified { get; set; }
        public int TotalQuote { get; set; }
        public int TotalClosure { get; set; }
        public int TotalWon { get; set; }
        public int TotalLost { get; set; }

        public SaleType? SaleType { get; set; }
        public PriorityType? PriorityType { get; set; }   
    }
}