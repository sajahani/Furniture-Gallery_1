using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Furniture_Gallery.Models
{
    public partial class Furnitureorder
    {
        public Furnitureorder()
        {
            Productorders = new HashSet<Productorder>();
        }

        public decimal Id { get; set; }
        public DateTime? OrderDate { get; set; }
        public string OrderStatus { get; set; }
        public decimal? OrderTotal { get; set; }
        public decimal? UseraccountId { get; set; }

        public virtual Useraccount Useraccount { get; set; }
        public virtual ICollection<Productorder> Productorders { get; set; }

        [NotMapped]
        public int Paid { get; set; }
        [NotMapped]
        public int Unpaid { get; set; }
    }
}
