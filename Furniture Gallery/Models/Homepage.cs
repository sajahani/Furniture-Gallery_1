using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Furniture_Gallery.Models
{
    public partial class Homepage
    {
        public decimal HomepageId { get; set; }
        public string HomepageTitle { get; set; }
        public string HomepageDescription { get; set; }

        [NotMapped]
        public virtual IFormFile background_Image { get; set; }
        public string HomepageImage { get; set; }

        [NotMapped]
        public virtual IFormFile Logo_Image { get; set; }
        public string HomepageLogo { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string EmailFacebook { get; set; }
    }
}
