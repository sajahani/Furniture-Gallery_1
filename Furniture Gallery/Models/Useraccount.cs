using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Furniture_Gallery.Models
{
    public partial class Useraccount
    {
        public Useraccount()
        {
            Furnitureorders = new HashSet<Furnitureorder>();
            Testimonials = new HashSet<Testimonial>();
            Furniturepayments = new HashSet<Furniturepayment>();
        }

        public decimal Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }


        [NotMapped]
        public virtual IFormFile ImageUser { get; set; }
        public string ImagePath { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public decimal? RoleId { get; set; }

        public virtual Furniturerole Role { get; set; }
        public virtual ICollection<Furnitureorder> Furnitureorders { get; set; }
        public virtual ICollection<Testimonial> Testimonials { get; set; }
		
        public virtual ICollection<Furniturepayment> Furniturepayments { get; set; }
	}
}
