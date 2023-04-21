using System;
using System.Collections.Generic;

#nullable disable

namespace Furniture_Gallery.Models
{
    public partial class BankAccInfo
    {
        public decimal? Cardnumber { get; set; }
        public decimal? Cvv { get; set; }
        public decimal? UseraccountId { get; set; }

        public virtual Useraccount Useraccount { get; set; }
    }
}
