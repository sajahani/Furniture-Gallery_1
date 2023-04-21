using Furniture_Gallery.Controllers;
using System;
using System.Threading.Tasks;

namespace Furniture_Gallery.Models
{
    public class Join2
    {   
        public Productorder productorder { get; set; }
        public Furnitureorder order { get; set;}
        public Furnitureproduct product { get; set; }

        public Furniturepayment payment { get; set; }

      
    }
}
