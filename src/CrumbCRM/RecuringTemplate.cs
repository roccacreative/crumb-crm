using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrumbCRM
{
    class RecuringTemplate
    {
        public int RecuringID { get; set; }
        public int ClientID { get; set; }
        public string Name { get; set; }
        public DateTime NextDate { get; set; }
        public DateTime Enabled { get; set; }
        public bool AutoSend { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
