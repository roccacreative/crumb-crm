using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrumbCRM.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CrumbCRM.Security;

namespace CrumbCRM
{
    public class Sale
    {
        [Key]
        [Column("SaleID")]
        public int ID { get; set; }

        public string JobTitle { get; set; }
        public string Name { get; set; }

        public int CompanyID { get; set; }

        [NotMapped]
        public Contact Company { get; set; }

        public int PersonID { get; set; }

        [ForeignKey("PersonID")]
        public Contact Person { get; set; }

        public int? OwnerID { get; set; }

        [ForeignKey("OwnerID")]
        public User OwnerUser { get; set; }

        public SaleType? Status { get; set; }
        public PriorityType? Priority { get; set; }
        
        public List<Note> Notes { get; set; }
        
               
        public DateTime ModifiedDate { get; set; }
        
        public DateTime CreatedDate { get; set; }
        
        public DateTime? Deleted { get; set; }

        public decimal Value { get; set; }

        public virtual List<SaleTag> Tags { get; set; }

        public int? CampaignID { get; set; }

        [ForeignKey("CampaignID")]
        public Campaign Campaign { get; set; }
        [NotMapped]
        public Note LastNote { get; set; }

        public Sale()
        {
            CreatedDate = DateTime.Now;
            ModifiedDate = DateTime.Now;
            Tags = new List<SaleTag>();
        }
    }
}
