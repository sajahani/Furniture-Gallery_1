using Furniture_Gallery.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using System.Xml;

namespace Furniture_Gallery.Controllers
{
    public class ShoppingController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        public ShoppingController(ModelContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }
 
        [HttpGet]

        public ActionResult AddToItem(decimal id)
        {
			ViewBag.user = HttpContext.Session.GetString("fname");
			 decimal num = Convert.ToDecimal(HttpContext.Session.GetInt32("IdCustomer"));
			   var product =  _context.Furnitureproducts.Where(m => m.Id == id).FirstOrDefault();
            return View(product);
		}


		[HttpPost]
        [ValidateAntiForgeryToken]
        public  async Task<ActionResult> AddToItem(Productorder productorder)
        {
            ViewBag.user = HttpContext.Session.GetString("fname");

            productorder.Id = 0;
            // Get the current user's id
            var userId = Convert.ToDecimal(HttpContext.Session.GetInt32("IdCustomer"));

            // Create a new Order object
            var order = new Furnitureorder
            {
                OrderDate = DateTime.Now,
                OrderStatus = "UnPaid",
                UseraccountId = userId
            };

            
              _context.Furnitureorders.Add(order);
                 await _context.SaveChangesAsync();

            // Update the ProductOrder object with the OrderId
            productorder.OrderId = order.Id;
           
            productorder.TotalAmount = productorder.Quantity *  _context.Furnitureproducts.Find(productorder.ProductId).Price;

            // Add the ProductOrder object to the database
            _context.Productorders.Add(productorder);
              await _context.SaveChangesAsync();

            return RedirectToAction("Join2", "Shopping");
        }

        [HttpGet]
		public ActionResult Join2(decimal userId)
		{
           

            ViewBag.user = HttpContext.Session.GetString("fname");
			userId = Convert.ToDecimal(HttpContext.Session.GetInt32("IdCustomer"));
			var orders = _context.Furnitureorders.ToList();
            var products = _context.Furnitureproducts.ToList();
            var productorders = _context.Productorders.ToList();

			var allorder = (from o in _context.Furnitureorders
						 join po in _context.Productorders on o.Id equals po.OrderId
						 join p in _context.Furnitureproducts on po.ProductId equals p.Id
                        
						
						 where o.UseraccountId == userId && o.OrderStatus == "UnPaid" 
						 select new Join2
						 {
							 order = o,
							 product = p,
							 productorder = po ,
                           
						 }).ToList();

			if (allorder == null)
			{
				return NotFound();
			}
			else
			{
				return View(allorder);
			}
		}





        [HttpGet]

        public IActionResult CheckOut()
        {
            return View();
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CheckOut(Join2 join2,decimal id)
        {

			ViewBag.user = HttpContext.Session.GetString("fname");
			 Convert.ToDecimal(HttpContext.Session.GetInt32("IdCustomer"));
			var order2 = _context.Productorders.FirstOrDefault(o => o.Id == id && o.Order.OrderStatus == "UnPaid");
            decimal card =0;
            decimal cvv = 0;

            Bank bank = new Bank ();

            if (card == bank.CreditCard && cvv == bank.Cvv)
            {
                if (order2.TotalAmount > bank.Balance)
                {
                    return RedirectToAction("InsufficientFunds");
                }
                else
                {
                    join2.order.OrderStatus = "Paid";
                    bank.Balance = bank.Balance - order2.TotalAmount;


                    _context.SaveChanges();
                }
            }

			var payment = new Furniturepayment
			{
				
			    PaymentAmount = order2.TotalAmount,
				PaymentDate = DateTime.Now
			};
			_context.Furniturepayments.Add(payment);
			_context.SaveChanges();
           
            order2.PaymentId = payment.Id;

			//_context.Productorders.Add();
			_context.SaveChanges();

			return View();
		}



	}
}
