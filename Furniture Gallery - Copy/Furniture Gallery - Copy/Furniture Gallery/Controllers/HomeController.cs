using Furniture_Gallery.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Session;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Furniture_Gallery.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ModelContext _context;

        List<Productorder> li = new List<Productorder> ();
		

		//public object Session { get; private set; }


		public HomeController(ILogger<HomeController> logger , ModelContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.logo = HttpContext.Session.GetString("logo");
            
            var home = await _context.Homepages.ToListAsync();
            var about = await _context.AboutUs.ToListAsync();
            var contact = await _context.ContactUs.ToListAsync();
            var category = await _context.Furniturecategories.ToListAsync();
			var testimoial = await _context.Testimonials.Include(x => x.Useraccount).ToListAsync();
            ContactU contact1= new ContactU();

            var homepages2 = Tuple.Create<IEnumerable<Homepage>, IEnumerable<AboutU>,IEnumerable<ContactU>,IEnumerable<Furniturecategory>,IEnumerable<Testimonial>>(home,about,contact,category,testimoial);

            
           
            return View(homepages2);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ContactId,FullName,Email,Message")] ContactU contactU, string Name, string Email, string Massage)
        {
            if (ModelState.IsValid)
            {
                contactU.FullName = Name;
                contactU.Email = Email;
                contactU.Message = Massage;

                _context.Add(contactU);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(contactU);
        }

        public  async Task<IActionResult> GetProduct()
        {
            ViewBag.logo = HttpContext.Session.GetString("logo");
            ViewBag.user = HttpContext.Session.GetString("fname");
            decimal num = Convert.ToDecimal(HttpContext.Session.GetInt32("IdCustomer"));
            var product = await _context.Furnitureproducts.Include(x=>x.Category).ToListAsync();   
            return View(product);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Filter(string searchString)
        {
            ViewBag.user = HttpContext.Session.GetString("fname");
			decimal num = Convert.ToDecimal(HttpContext.Session.GetInt32("IdCustomer"));
			var allProducts = await _context.Furnitureproducts.Include(n => n.Category).ToListAsync();


            if (!string.IsNullOrEmpty(searchString))
            {
                var filteredResult = allProducts.Where(n => n.Productname.ToLower().Contains(searchString.ToLower()) || n.Category.Categoryname.ToLower().Contains(searchString.ToLower())).ToList();

            

                return View("GetProduct", filteredResult);
            }
			ViewBag.user = HttpContext.Session.GetString("fname");
			Convert.ToDecimal(HttpContext.Session.GetInt32("IdCustomer"));
			return View("GetProduct", allProducts);
        }
		
        

		//public async Task<IActionResult> AddItemToShoppingCart(decimal Id)
		//{
		//    var detailproduct = await _context.Productorders.Include(c => c.Product).Include(c => c.Order).FirstOrDefaultAsync(c => c.ProductId == Id);
		//    return View(detailproduct);
		//}

		//public async Task<IActionResult> AddToItem(decimal Id)
		//{

		//    var products = await _context.Furnitureproducts.ToListAsync();
		//    var orders = await _context.Furnitureorders.ToListAsync();
		//    var productOrder = await _context.Productorders.ToListAsync();
		//    var customers = await _context.Useraccounts.ToListAsync();

		//    var all = from o in orders
		//              join po in productOrder on o.Id equals po.OrderId
		//              join p in products on po.ProductId equals p.Id
		//              select new Join2 { order = o, productorder = po, product = p };
		//    var addpro = all.FirstOrDefault(c => c.productorder.ProductId == Id);

		//    return View(addpro);
		//    //var AddCart = await  _context.Productorders.Include(c => c.Product).Include(c => c.Order).FirstOrDefaultAsync(c => c.ProductId == Id);
		//    //return View(AddCart);

		//}





	

		public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


		
    }
}
