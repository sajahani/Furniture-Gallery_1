namespace Furniture_Gallery.Models
{
    public class OrderChartData
    {
        public string Label { get; set; }
        public decimal Value { get; set; }

        public Furniturecategory Category { get; set; } 

        public string CategoryName { get; set; }
        public Furnitureproduct Product { get; set; }
        public decimal ProductCount { get; set; }   
    }
}

