using CrumbCRM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CrumbCRM.Web.Models
{
    public class LeadItemViewModel
    {
        public Lead Lead { get; set; }

        public Note Note { get; set; }

        public List<Note> Notes { get; set; }
    }
}