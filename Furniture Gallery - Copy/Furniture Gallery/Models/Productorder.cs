using System;
using System.Collections.Generic;

#nullable disable

namespace Furniture_Gallery.Models
{
    public partial class Productorder
    {
        public decimal Id { get; set; }
        public decimal? OrderId { get; set; }
        public decimal? ProductId { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? PaymentId { get; set; }

        public decimal? TotalAmount { get; set; }   
		
		public virtual Furnitureorder Order { get; set; }
        public virtual Furniturepayment Payment { get; set; }
        public virtual Furnitureproduct Product { get; set; }
    }
}
