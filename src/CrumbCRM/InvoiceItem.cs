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
    public class InvoiceItem
    {
        [Key]
        [Column("InvoiceItemID")]
        public int ID { get; set; }
        public int InvoiceID { get; set; }
        public string Title { get; set; }
        public decimal Value { get; set; }
        public DateTime Date { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? Deleted { get; set; }

        public InvoiceItem()
        {
            CreatedDate = DateTime.Now;
            Date = DateTime.Now;
        }
    }
}
