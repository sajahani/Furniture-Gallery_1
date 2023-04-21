using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

#nullable disable

namespace Furniture_Gallery.Models
{
    public partial class Furniturecategory
    {
        public Furniturecategory() {

            Furnitureproducts = new HashSet<Furnitureproduct>();

        }
        public decimal Id { get; set; }
        [Display(Name = "Category Name")]
        public string Categoryname { get; set; }

        [NotMapped]
        [Display(Name = "Category Image")]
        public virtual IFormFile ImageCat { get; set; }

        [Display(Name = "Category Image")]
        public string ImagePath { get; set; }

        public virtual ICollection<Furnitureproduct> Furnitureproducts { get; set; }
    }
}
