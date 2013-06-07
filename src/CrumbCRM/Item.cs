using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrumbCRM
{
    public class Item 
    {
        public int Quantity { get; set; }
        public ItemType Type { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int TotalPrice { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Item()
        {
            CreatedDate = DateTime.Now;
        }
    }
}
