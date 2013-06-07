using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrumbCRM.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrumbCRM
{
    public class Quote
    {
        [Key]
        [Column("QuoteID")]
        public int ID { get; set; }
        public int ClientID { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Notes { get; set; }
        public string Terms { get; set; }
        public StatusType Status { get; set; }
        public string StatusDetails { get; set;}
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Deleted { get; set; }
        public Quote()
        {
            CreatedDate = DateTime.Now;
            Date = DateTime.Now;
        }
    }
}
