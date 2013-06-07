using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrumbCRM.Security
{
    public class Role
    {
        [Key]
        public virtual Guid RoleId { get; set; }

        [Required]
        public virtual string RoleName { get; set; }

        public virtual string Description { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
