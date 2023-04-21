using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

#nullable disable

namespace Furniture_Gallery.Models
{
    public partial class Furnitureproduct
    {
        public Furnitureproduct()
        {
            Productorders = new HashSet<Productorder>();
        }

        public decimal Id { get; set; }
        [Display(Name = "Product Name")]
        public string Productname { get; set; }
        public string Description { get; set; }
        public decimal? Price { get; set; }

        [NotMapped]
        [Display(Name = "Product Image")]

        public virtual IFormFile ImagePro { get; set; }
        [Display(Name = "Product Image")]
        public string ImagePath { get; set; }
        

        [Display(Name = "Category Name")]

        public decimal? CategoryId { get; set; }
        public virtual Furniturecategory Category { get; set; }
      
        public virtual ICollection<Productorder> Productorders { get; set; }
    }
}
