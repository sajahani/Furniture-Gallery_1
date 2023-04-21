using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Furniture_Gallery.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Components;

namespace Furniture_Gallery.Controllers
{
    public class HomepagesController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        public HomepagesController(ModelContext context , IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        


        // GET: Homepages
        public async Task<IActionResult> Index()
        {  

            ViewBag.username = HttpContext.Session.GetString("UserName");
            return View(await _context.Homepages.ToListAsync());
        }

        // GET: Homepages/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var homepage = await _context.Homepages
                .FirstOrDefaultAsync(m => m.HomepageId == id);
            if (homepage == null)
            {
                return NotFound();
            }

            return View(homepage);
        }

        // GET: Homepages/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Homepages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HomepageId,HomepageTitle,HomepageDescription,HomepageImage,HomepageLogo,Phone,Email,EmailFacebook,background_Image ,Logo_Image")] Homepage homepage)
        {
            if (ModelState.IsValid)
            {
                if (homepage.background_Image != null)
                {

                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string imageName = Guid.NewGuid().ToString() + "_" + homepage.background_Image.FileName;
                    string path = Path.Combine(wwwRootPath + "/Images/", imageName);

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await homepage.background_Image.CopyToAsync(fileStream);
                    }
                   homepage.HomepageImage = imageName;
                }
             

                if (homepage.Logo_Image != null)
                {

                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string imageName = Guid.NewGuid().ToString() + "_" + homepage.Logo_Image.FileName;
                    string path = Path.Combine(wwwRootPath + "/Images/", imageName);

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await homepage.Logo_Image.CopyToAsync(fileStream);
                    }
                    homepage.HomepageLogo = imageName;
                }
                HttpContext.Session.SetString("logo", homepage.HomepageTitle);
                _context.Add(homepage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(homepage);
        }

        // GET: Homepages/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            ViewBag.username = HttpContext.Session.GetString("UserName");
            if (id == null)
            {
                return NotFound();
            }

            var homepage = await _context.Homepages.FindAsync(id);
            if (homepage == null)
            {
                return NotFound();
            }
            return View(homepage);
        }

        // POST: Homepages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("HomepageId,HomepageTitle,HomepageDescription,HomepageImage,HomepageLogo,Phone,Email,EmailFacebook,background_Image ,Logo_Image")] Homepage homepage)
        {
            if (id != homepage.HomepageId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (homepage.background_Image != null)
                {
                    string HomepageImage = homepage.HomepageImage;
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string imageName = Guid.NewGuid().ToString() + "_" + homepage.background_Image.FileName;
                    string path = Path.Combine(wwwRootPath + "/Images/", imageName);

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await homepage.background_Image.CopyToAsync(fileStream);
                        HomepageImage = @"\images\" + imageName;
                    }
                    homepage.HomepageImage = imageName;
                }


                if (homepage.Logo_Image != null)
                {
                    string HomepageLogo = homepage.HomepageLogo;
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string imageName = Guid.NewGuid().ToString() + "_" + homepage.Logo_Image.FileName;
                    string path = Path.Combine(wwwRootPath + "/Images/", imageName);

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await homepage.Logo_Image.CopyToAsync(fileStream);
                        HomepageLogo = @"\images\" + imageName;

                    }
                    homepage.HomepageLogo = imageName;
                }
                try
                {
                    _context.Update(homepage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HomepageExists(homepage.HomepageId))
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
            return View(homepage);
        }

        // GET: Homepages/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var homepage = await _context.Homepages
                .FirstOrDefaultAsync(m => m.HomepageId == id);
            if (homepage == null)
            {
                return NotFound();
            }

            return View(homepage);
        }

        // POST: Homepages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            var homepage = await _context.Homepages.FindAsync(id);
            _context.Homepages.Remove(homepage);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HomepageExists(decimal id)
        {
            return _context.Homepages.Any(e => e.HomepageId == id);
        }
    }
}
