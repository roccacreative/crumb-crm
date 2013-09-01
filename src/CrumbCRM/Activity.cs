using CrumbCRM.Enums;
using CrumbCRM.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrumbCRM
{
    public class Activity
    {
        [Column("ActivityID")]
        public int ID { get; set; }

        public string Description { get; set; }

        public AreaType Type { get; set; }

        public int? ItemID { get; set; }

        public int? UserID { get; set; }

        [ForeignKey("UserID")]
        public User User { get; set; }

        public DateTime ActivityDate { get; set; }

        public Activity()
        {
            ActivityDate = DateTime.Now;
        }
    }
}
