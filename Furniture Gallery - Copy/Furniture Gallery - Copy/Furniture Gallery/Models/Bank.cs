using System;
using System.Collections.Generic;

#nullable disable

namespace Furniture_Gallery.Models
{
    public partial class Bank
    {
        public decimal Id { get; set; }
        public decimal? CreditCard { get; set; }
        public decimal? Balance { get; set; }
        public decimal? Cvv { get; set; }
        public DateTime? ExpireDate { get; set; }
    }
}
