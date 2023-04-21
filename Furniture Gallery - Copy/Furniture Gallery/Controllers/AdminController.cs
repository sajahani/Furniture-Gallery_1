
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

            //var data = (from p in _context.Furnitureproducts
            //           join c in _context.Furniturecategories on p.CategoryId equals c.Id
            //           group p by c.Categoryname into g
            //           select new  { Category = g.Key, ProductCount = g.Count() })
            //           .ToDictionary(x => x.Category, x => x.ProductCount); 


            //var data = (from p in _context.Furnitureproducts
            //            join c in _context.Furniturecategories
            //            on p.CategoryId equals c.Id
            //            group p by p.Category into g
            //            select new OrderChartData
            //            {
            //                Category = g.Key.Categoryname,
            //                ProductCount = g.Count(),

            //            }).ToList();
            return View();

           
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
