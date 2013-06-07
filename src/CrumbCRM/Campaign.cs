using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrumbCRM
{
    public class Campaign
    {
        [Key]
        [Column("CampaignID")]        
        public int ID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? Deleted { get; set; }

        public Guid CreatedByID { get; set; }
    }
}
