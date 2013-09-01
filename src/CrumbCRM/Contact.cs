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
    public class Contact
    {
        [Key]
        [Column("ContactID")]
        public int ID { get; set; }
        
        public int? DepartmentID { get; set; }
        
        public Guid? OwnerID { get; set; }

        [ForeignKey("OwnerID")]
        public User OwnerUser { get; set; }
        
        public ContactType Type { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string BusinessSector { get; set; }
        public string JobTitle { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Work { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string Postcode { get; set; }
        [Required]
        public bool Private { get; set; }
        public DateTime ModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? Deleted { get; set; }

        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }

        public int? CompanyID { get; set; }

        [ForeignKey("CompanyID")]
        public Contact Company { get; set; }

        public Contact()
        {
            CreatedDate = DateTime.Now;
            ModifiedDate = DateTime.Now;
        }        
    }
}
