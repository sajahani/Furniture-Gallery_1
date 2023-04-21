using System;
using System.Collections.Generic;

#nullable disable

namespace Furniture_Gallery.Models
{
    public partial class ContactU
    {
        public decimal ContactId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
    }
}
