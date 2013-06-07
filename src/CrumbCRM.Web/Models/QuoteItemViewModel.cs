using CrumbCRM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CrumbCRM.Web.Models
{
    public class QuoteItemViewModel
    {
        public Quote Quote { get; set; }
        public QuoteItem QuoteItem { get; set; }
        public List<QuoteItem> QuoteItems { get; set; }
        public Note Note { get; set; }
        public List<Note> Notes { get; set; }
    }
}