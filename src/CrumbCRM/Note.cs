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
    public class Note
    {
        [Key]
        [Column("ID")]
        public int ID { get; set; }
        public Guid AuthorID { get; set; }        
        public int ItemID { get; set; }        
        public NoteType Type { get; set; }
        public NoteActionType Action { get; set; }
        public string Body { get; set; }        
        public DateTime ModifiedDate { get; set; }        
        public DateTime CreatedDate { get; set; }        
        public DateTime? Deleted { get; set; }

        public Note()
        {
            CreatedDate = DateTime.Now;
            ModifiedDate = DateTime.Now;
            Action = NoteActionType.General;
        }
    }
}
