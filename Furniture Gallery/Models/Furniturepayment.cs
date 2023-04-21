using System;
using System.Collections.Generic;

#nullable disable

namespace Furniture_Gallery.Models
{
    public partial class Furniturepayment
    {
        public Furniturepayment()
        {
            Productorders = new HashSet<Productorder>();
        }

        public decimal Id { get; set; }
        public DateTime? PaymentDate { get; set; }
        public decimal? PaymentAmount { get; set; }

       public decimal? UseraId { get; set; }

        public virtual Useraccount Useraccount { get; set; }

		public virtual ICollection<Productorder> Productorders { get; set; }
		
	}
}
