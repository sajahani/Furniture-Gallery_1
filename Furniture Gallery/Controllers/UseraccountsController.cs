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
    public class UseraccountsController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public UseraccountsController(ModelContext context , IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment= hostEnvironment;
        }

        // GET: Useraccounts
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Useraccounts.Include(u => u.Role);
            return View(await modelContext.ToListAsync());
        }

        // GET: Useraccounts/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var useraccount = await _context.Useraccounts
                .Include(u => u.Role)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (useraccount == null)
            {
                return NotFound();
            }

            return View(useraccount);
        }

        // GET: Useraccounts/Create
        public IActionResult Create()
        {
            ViewData["RoleId"] = new SelectList(_context.Furnitureroles, "Id", "Rolename");
            return View();
        }

        // POST: Useraccounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Firstname,Lastname,Email,ImagePath,Username,Password,RoleId ,ImageUser")] Useraccount useraccount)
        {
            if (ModelState.IsValid)
            {
                if (useraccount.ImageUser != null)
                {

                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string imageName = Guid.NewGuid().ToString() + "_" + useraccount.ImageUser.FileName;
                    string path = Path.Combine(wwwRootPath + "/Images/", imageName);

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await useraccount.ImageUser.CopyToAsync(fileStream);
                    }
                       useraccount.ImagePath = imageName;
                }
                _context.Add(useraccount);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoleId"] = new SelectList(_context.Furnitureroles, "Id", "Rolename", useraccount.RoleId);
            return View(useraccount);
        }

        // GET: Useraccounts/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var useraccount = await _context.Useraccounts.FindAsync(id);
            if (useraccount == null)
            {
                return NotFound();
            }
            ViewData["RoleId"] = new SelectList(_context.Furnitureroles, "Id", "Id", useraccount.RoleId);
            return View(useraccount);
        }

        // POST: Useraccounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,Firstname,Lastname,Email,ImagePath,Username,Password,RoleId ,ImageUser")] Useraccount useraccount)
        {
            if (id != useraccount.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (useraccount.ImageUser != null)
                {
                    string ImagePath =useraccount.ImagePath;
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
                try
                {
                    _context.Update(useraccount);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UseraccountExists(useraccount.Id))
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
            ViewData["RoleId"] = new SelectList(_context.Furnitureroles, "Id", "Id", useraccount.RoleId);
            return View(useraccount);
        }

        // GET: Useraccounts/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var useraccount = await _context.Useraccounts
                .Include(u => u.Role)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (useraccount == null)
            {
                return NotFound();
            }

            return View(useraccount);
        }

        // POST: Useraccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            var useraccount = await _context.Useraccounts.FindAsync(id);
            _context.Useraccounts.Remove(useraccount);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UseraccountExists(decimal id)
        {
            return _context.Useraccounts.Any(e => e.Id == id);
        }






        [HttpGet]
        public async Task<IActionResult> UserProfile(decimal? id)
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
        public async Task<IActionResult> UserProfile(decimal id, [Bind("Id,Firstname,Lastname,Email,ImagePath,Username,Password,RoleId ,ImageUser")] Useraccount useraccount)
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


                return RedirectToAction("UserProfile", "Useraccounts");
            }
            ViewData["RoleId"] = new SelectList(_context.Furnitureroles, "Id", "Rolename", useraccount.RoleId);
            return View(useraccount);
        }





    }
}
