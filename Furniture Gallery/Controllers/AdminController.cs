
using Furniture_Gallery.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Dynamic;

namespace Furniture_Gallery.Controllers
{
    public class AdminController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public AdminController (ModelContext context , IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }
        public IActionResult Index()
        {
            ViewBag.username = HttpContext.Session.GetString("UserName");

            ViewBag.usernum = _context.Useraccounts.Count();
            ViewBag.catnum = _context.Furniturecategories.Count();
            ViewBag.productnum = _context.Furnitureproducts.Count();
            ViewBag.ordernum = _context.Furnitureorders.Count();
			
			var data = (from p in _context.Furnitureproducts
						join c in _context.Furniturecategories on p.CategoryId equals c.Id
						group p by c.Categoryname into g
						select new OrderChartData { Category = g.Key, Count = g.Count() }).ToList();

            var lastuser = _context.Useraccounts.OrderByDescending(x => x.Id ).Where(x => x.RoleId == 3).Take(5).ToList();




            //var orders = _context.Furnitureorders.ToList();
            // var paidOrders = orders.Where(o => o.OrderStatus == "Paid").Count();
            // var unpaidOrders = orders.Where(o => o.OrderStatus == "UnPaid").Count();


            //double percentagePaidOrders = (double)paidOrders / orders.Count() * 100;
            // double percentageUnpaidOrders = (double)unpaidOrders / orders.Count() * 100;


            // List<double> percentages = new List<double> { percentagePaidOrders, percentageUnpaidOrders };

            // Create a new object to store the paid and unpaid order counts
            //  var paidanduppaid = new Furnitureorder { Paid = paidOrders, Unpaid = unpaidOrders };

            var home = _context.Homepages.ToList();

            var indexadmin = Tuple.Create<IEnumerable<OrderChartData>,IEnumerable<Useraccount>,IEnumerable<Homepage>>(data,lastuser,home); 
           

            return View(indexadmin);

           
        }



        

        [HttpGet]
        public IActionResult Search()
        {
            var modelData = _context.Productorders.Include(p => p.Product).Include(c => c.Order).ToList();
            return View(modelData);
        }
  


        [HttpGet]
        public async Task<IActionResult> AdminProfile(decimal? id)
        {
            ViewBag.username = HttpContext.Session.GetString("UserName");

            id = Convert.ToDecimal(HttpContext.Session.GetInt32("IdCustomer"));
            if (id == null)
            {
                return NotFound();
            }

           // var useraccount = await _context.Useraccounts.FindAsync(id);
            var useraccount = await _context.Useraccounts
               .Include(u => u.Role)
               .FirstOrDefaultAsync(m => m.Id == id);


            if (useraccount == null)
            {
                return NotFound();
            }
            ViewData["RoleId"] = new SelectList(_context.Furnitureroles, "Id", "Rolename", useraccount.RoleId);
            return View(useraccount);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminProfile(decimal id, [Bind("Id,Firstname,Lastname,Email,ImagePath,Username,Password,RoleId ,ImageUser")] Useraccount useraccount)
        {
            id = Convert.ToDecimal(HttpContext.Session.GetInt32("IdCustomer"));
            if (id != useraccount.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (useraccount.ImageUser != null)
                {
                    string ImagePath = useraccount.ImagePath;
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string imageName = Guid.NewGuid().ToString() + "_" + useraccount.ImageUser.FileName;
                    string path = Path.Combine(wwwRootPath + "/Images/", imageName);

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await useraccount.ImageUser.CopyToAsync(fileStream);
                        ImagePath = @"\images\" + imageName;
                    }
                    useraccount.ImagePath = imageName;
                }
               
                    _context.Update(useraccount);
                    await _context.SaveChangesAsync();



                TempData["Message"] = "Your profile data has been saved successfully.";


                return RedirectToAction("AdminProfile", "Admin");
            }
            ViewData["RoleId"] = new SelectList(_context.Furnitureroles, "Id", "Rolename", useraccount.RoleId);
            return View(useraccount);
        }

        private bool UseraccountExists(decimal id)
        {
            throw new NotImplementedException();
        }





    }
}
