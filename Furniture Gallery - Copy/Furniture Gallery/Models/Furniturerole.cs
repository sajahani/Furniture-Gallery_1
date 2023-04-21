using System;
using System.Collections.Generic;

#nullable disable

namespace Furniture_Gallery.Models
{
    public partial class Furniturerole
    {
        public Furniturerole()
        {
            Useraccounts = new HashSet<Useraccount>();
        }

        public decimal Id { get; set; }
        public string Rolename { get; set; }

        public virtual ICollection<Useraccount> Useraccounts { get; set; }
    }
}
