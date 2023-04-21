using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Furniture_Gallery.Models
{
    public partial class AboutU
    {
        public decimal AboutusId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        [NotMapped]
       public virtual IFormFile ImageAboutUs { get; set; }

        [Display(Name = "Image_About_Us")]
        public string ImagePath { get; set; }
    }
}
