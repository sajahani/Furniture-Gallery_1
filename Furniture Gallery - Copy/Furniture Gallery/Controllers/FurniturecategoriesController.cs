using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Furniture_Gallery.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace Furniture_Gallery.Controllers
{
    public class FurniturecategoriesController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public FurniturecategoriesController(ModelContext context , IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment=hostEnvironment;
        }

        // GET: Furniturecategories
        public async Task<IActionResult> Index()
        {
            ViewBag.username = HttpContext.Session.GetString("UserName");
            return View(await _context.Furniturecategories.ToListAsync());
        }

        // GET: Furniturecategories/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            ViewBag.username = HttpContext.Session.GetString("UserName");
            if (id == null)
            {
                return NotFound();
            }

            var furniturecategory = await _context.Furniturecategories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (furniturecategory == null)
            {
                return NotFound();
            }

            return View(furniturecategory);
        }

        // GET: Furniturecategories/Create
        public IActionResult Create()
        {
            ViewBag.username = HttpContext.Session.GetString("UserName");
            return View();
        }

        // POST: Furniturecategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Categoryname,ImagePath,ImageCat")] Furniturecategory furniturecategory)
        {  
            if (ModelState.IsValid)
            {
                if (furniturecategory.ImageCat != null)
                {

                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string imageName = Guid.NewGuid().ToString() + "_" + furniturecategory.ImageCat.FileName;
                    string path = Path.Combine(wwwRootPath + "/Images/", imageName);

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await furniturecategory.ImageCat.CopyToAsync(fileStream);
                    }
                    furniturecategory.ImagePath = imageName;
                }
                _context.Add(furniturecategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(furniturecategory);
        }

        // GET: Furniturecategories/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            ViewBag.username = HttpContext.Session.GetString("UserName");
            if (id == null)
            {
                return NotFound();
            }

            var furniturecategory = await _context.Furniturecategories.FindAsync(id);
            if (furniturecategory == null)
            {
                return NotFound();
            }
            return View(furniturecategory);
        }

        // POST: Furniturecategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,Categoryname,ImagePath,ImageCat")] Furniturecategory furniturecategory)
        {
            if (id != furniturecategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (furniturecategory.ImageCat != null)
                {
                    string ImagePath = furniturecategory.ImagePath;
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string imageName = Guid.NewGuid().ToString() + "_" + furniturecategory.ImageCat.FileName;
                    string path = Path.Combine(wwwRootPath + "/Images/", imageName);

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await furniturecategory.ImageCat.CopyToAsync(fileStream);
                        ImagePath = @"\images\" + imageName;
                    }
                    furniturecategory.ImagePath = imageName;
                }
                try
                {
                    _context.Update(furniturecategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FurniturecategoryExists(furniturecategory.Id))
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
            return View(furniturecategory);
        }

        // GET: Furniturecategories/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            ViewBag.username = HttpContext.Session.GetString("UserName");
            if (id == null)
            {
                return NotFound();
            }

            var furniturecategory = await _context.Furniturecategories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (furniturecategory == null)
            {
                return NotFound();
            }

            return View(furniturecategory);
        }

        // POST: Furniturecategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            var furniturecategory = await _context.Furniturecategories.FindAsync(id);
            _context.Furniturecategories.Remove(furniturecategory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FurniturecategoryExists(decimal id)
        {
            return _context.Furniturecategories.Any(e => e.Id == id);
        }
    }
}
