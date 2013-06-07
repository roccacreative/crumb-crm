using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CrumbCRM.Web.Models
{
    public class NoteViewModel
    {
        public List<Note> Notes { get; set; }
        public int ItemID { get; set; }
        public NoteType NoteType { get; set; }
        public Note Note { get; set; }

        public dynamic ParentItem { get; set; }
    }
}