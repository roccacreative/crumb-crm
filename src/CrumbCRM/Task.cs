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
    public class Task
    {
        [Key]
        [Column("TaskID")]
        public int ID { get; set; }       
        
        public int? AssignedID { get; set; }
        
        [ForeignKey("AssignedID")]
        
        public User AssignedUser { get; set; }
        
        public int? ItemID { get; set; }
        
        public AreaType? AreaType { get; set; }
        
        public ActionType ActionType { get; set; }

        [Required]
        public string Title { get; set; }

        public string Body { get; set; }        
        
        public DateTime? Active { get; set; }        
        
        public int? DeactivatedBy { get; set; }        
        
        public DateTime? DueDate { get; set; }        
        
        public DateTime ModifiedDate { get; set; }        
        
        public DateTime CreatedDate { get; set; } 
        
        public DateTime? Deleted { get; set; }
        
        public int AssignedByID { get; set; }
        
        [ForeignKey("AssignedByID")]
        public User AssignedByUser { get; set; }

        public bool AllDay { get; set; }

        public Task()
        {
            CreatedDate = DateTime.Now;
            ModifiedDate = DateTime.Now;
            AllDay = true;
        }
    }
}
