using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Furniture_Gallery.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Furniture_Gallery.Controllers
{
    public class AboutUsController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public AboutUsController(ModelContext context , IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // GET: AboutUs
        public async Task<IActionResult> Index()
        {
            ViewBag.username = HttpContext.Session.GetString("UserName");
            return View(await _context.AboutUs.ToListAsync());
        }

        // GET: AboutUs/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aboutU = await _context.AboutUs
                .FirstOrDefaultAsync(m => m.AboutusId == id);
            if (aboutU == null)
            {
                return NotFound();
            }

            return View(aboutU);
        }

        // GET: AboutUs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AboutUs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AboutusId,Title,Description,ImagePath,ImageAboutUs")] AboutU aboutU)
        {
            if (ModelState.IsValid)
            {


                if (aboutU.ImageAboutUs != null)
                {

                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string imageName = Guid.NewGuid().ToString() + "_" + aboutU.ImageAboutUs.FileName;
                    string path = Path.Combine(wwwRootPath + "/Images/", imageName);

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await aboutU.ImageAboutUs.CopyToAsync(fileStream);
                    }
                       aboutU.ImagePath = imageName;
                }



                _context.Add(aboutU);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(aboutU);
        }

        // GET: AboutUs/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            ViewBag.username = HttpContext.Session.GetString("UserName");
            if (id == null)
            {
                return NotFound();
            }

            var aboutU = await _context.AboutUs.FindAsync(id);
            if (aboutU == null)
            {
                return NotFound();
            }
            return View(aboutU);
        }

        // POST: AboutUs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("AboutusId,Title,Description,ImagePath,ImageAboutUs")] AboutU aboutU)
        {
            if (id != aboutU.AboutusId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                if (aboutU.ImageAboutUs != null)
                {
                    string ImagePath = aboutU.ImagePath;
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string imageName = Guid.NewGuid().ToString() + "_" + aboutU.ImageAboutUs.FileName;
                    string path = Path.Combine(wwwRootPath + "/Images/", imageName);

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await aboutU.ImageAboutUs.CopyToAsync(fileStream);
                        ImagePath = @"\images\" + imageName;
                    }
                    aboutU.ImagePath = imageName;
                }

                try
                {
                    _context.Update(aboutU);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AboutUExists(aboutU.AboutusId))
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
            return View(aboutU);
        }

        // GET: AboutUs/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aboutU = await _context.AboutUs
                .FirstOrDefaultAsync(m => m.AboutusId == id);
            if (aboutU == null)
            {
                return NotFound();
            }

            return View(aboutU);
        }

        // POST: AboutUs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            var aboutU = await _context.AboutUs.FindAsync(id);
            _context.AboutUs.Remove(aboutU);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AboutUExists(decimal id)
        {
            return _context.AboutUs.Any(e => e.AboutusId == id);
        }
    }
}
