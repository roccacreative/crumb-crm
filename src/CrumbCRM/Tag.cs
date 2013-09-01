using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrumbCRM
{
    public class Tag
    {
        [Key]
        [Column("TagID")]
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedByID { get; set; }
        public DateTime? Deleted { get; set; }
    }

    public class LeadTag
    {
        [Key, Column(Order = 0)]
        public int LeadID { get; set; }
        [Key, Column(Order = 1)]
        public int TagID { get; set; }

        [ForeignKey("LeadID")]
        public Lead Lead { get; set; }

        [ForeignKey("TagID")]
        public Tag Tag { get; set; }
    }

    public class SaleTag
    {
        [Key, Column(Order = 0)]
        public int SaleID { get; set; }
        [Key, Column(Order = 1)]
        public int TagID { get; set; }

        [ForeignKey("SaleID")]
        public Sale Sale { get; set; }

        [ForeignKey("TagID")]
        public Tag Tag { get; set; }
    }
}
