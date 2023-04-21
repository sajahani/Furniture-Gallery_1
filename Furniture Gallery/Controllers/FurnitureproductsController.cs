using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Furniture_Gallery.Models;
using Microsoft.Extensions.Hosting;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Furniture_Gallery.Controllers
{
    public class FurnitureproductsController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;


        public FurnitureproductsController(ModelContext context , IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // GET: Furnitureproducts
        public async Task<IActionResult> Index()
        {
            ViewBag.username = HttpContext.Session.GetString("UserName"); 
            var modelContext = _context.Furnitureproducts.Include(f => f.Category);
            return View(await modelContext.ToListAsync());
        }

        // GET: Furnitureproducts/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            ViewBag.username = HttpContext.Session.GetString("UserName");
            if (id == null)
            {
                return NotFound();
            }

            var furnitureproduct = await _context.Furnitureproducts
                .Include(f => f.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (furnitureproduct == null)
            {
                return NotFound();
            }

            return View(furnitureproduct);
        }

        // GET: Furnitureproducts/Create
        public IActionResult Create()
        {
            ViewBag.username = HttpContext.Session.GetString("UserName");
            ViewData["CategoryId"] = new SelectList(_context.Furniturecategories, "Id", "Categoryname");
            return View();
        }

        // POST: Furnitureproducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Productname,Description,Price,ImagePath,CategoryId,ImagePro")] Furnitureproduct furnitureproduct)
		{
			ViewBag.username = HttpContext.Session.GetString("UserName");
			if (ModelState.IsValid)
            {
                if (furnitureproduct.ImagePro != null)
                {

                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string imageName = Guid.NewGuid().ToString() + "_" + furnitureproduct.ImagePro.FileName;
                    string path = Path.Combine(wwwRootPath + "/Images/", imageName);

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await furnitureproduct.ImagePro.CopyToAsync(fileStream);
                    }
                    furnitureproduct.ImagePath = imageName;
                }
                _context.Add(furnitureproduct);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Furniturecategories, "Id", "Categoryname", furnitureproduct.CategoryId);
            return View(furnitureproduct);
        }

        // GET: Furnitureproducts/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
		{
			ViewBag.username = HttpContext.Session.GetString("UserName");
			if (id == null)
            {
                return NotFound();
            }

            var furnitureproduct = await _context.Furnitureproducts.FindAsync(id);
            if (furnitureproduct == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Furniturecategories, "Id", "Categoryname", furnitureproduct.CategoryId);
            return View(furnitureproduct);
        }

        // POST: Furnitureproducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,Productname,Description,Price,ImagePath,CategoryId,ImagePro")] Furnitureproduct furnitureproduct)
		{
			ViewBag.username = HttpContext.Session.GetString("UserName");
			if (id != furnitureproduct.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (furnitureproduct.ImagePro != null)
                {
                    string ImagePath = furnitureproduct.ImagePath;
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string imageName = Guid.NewGuid().ToString() + "_" + furnitureproduct.ImagePro.FileName;
                    string path = Path.Combine(wwwRootPath + "/Images/", imageName);

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await furnitureproduct.ImagePro.CopyToAsync(fileStream);
                        ImagePath = @"\images\" + imageName;
                    }
                    furnitureproduct.ImagePath = imageName;
                }
                try
                {
                    _context.Update(furnitureproduct);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FurnitureproductExists(furnitureproduct.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Furniturecategories, "Id", "Categoryname", furnitureproduct.CategoryId);
            return View(furnitureproduct);
        }

        // GET: Furnitureproducts/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            ViewBag.username = HttpContext.Session.GetString("UserName");
            if (id == null)
            {
                return NotFound();
            }

            var furnitureproduct = await _context.Furnitureproducts
                .Include(f => f.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (furnitureproduct == null)
            {
                return NotFound();
            }

            return View(furnitureproduct);
        }

        // POST: Furnitureproducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
		{
			ViewBag.username = HttpContext.Session.GetString("UserName");
			var furnitureproduct = await _context.Furnitureproducts.FindAsync(id);
            _context.Furnitureproducts.Remove(furnitureproduct);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FurnitureproductExists(decimal id)
        {
            return _context.Furnitureproducts.Any(e => e.Id == id);
        }
    }
}
