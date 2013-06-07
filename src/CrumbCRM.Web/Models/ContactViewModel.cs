using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CrumbCRM.Web.Models
{
    public class ContactViewModel
    {
        public List<Contact> Contacts { get; set; }
        public List<Task> Tasks { get; set; }
    }
}