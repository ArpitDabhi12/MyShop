using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Core.Models
{
    public class Product : BaseEntity
    {

        [StringLength(20)]
        [Display(Name = "Product Name")]
        public string Name { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }

        public string Category { get; set; }

        [Range(0, 1000)]
        public string Price { get; set; }

    }
}
