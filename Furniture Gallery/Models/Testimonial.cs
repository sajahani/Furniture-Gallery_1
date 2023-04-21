using System;
using System.Collections.Generic;

#nullable disable

namespace Furniture_Gallery.Models
{
    public partial class Testimonial
    {
        public decimal TestimonialId { get; set; }
        public decimal? UseraccountId { get; set; }
        public decimal? Rating { get; set; }
        public string Message { get; set; }
        public string TestimonialStatus { get; set; }

        public virtual Useraccount Useraccount { get; set; }
    }
}
