using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Furniture_Gallery.Models;

namespace Furniture_Gallery.Controllers
{
    public class FurnitureordersController : Controller
    {
        private readonly ModelContext _context;

        public FurnitureordersController(ModelContext context)
        {
            _context = context;
        }

        // GET: Furnitureorders
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Furnitureorders.Include(f => f.Useraccount);
            return View(await modelContext.ToListAsync());
        }

        // GET: Furnitureorders/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var furnitureorder = await _context.Furnitureorders
                .Include(f => f.Useraccount)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (furnitureorder == null)
            {
                return NotFound();
            }

            return View(furnitureorder);
        }

        // GET: Furnitureorders/Create
        public IActionResult Create()
        {
            ViewData["UseraccountId"] = new SelectList(_context.Useraccounts, "Id", "Id");
            return View();
        }

        // POST: Furnitureorders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,OrderDate,OrderStatus,OrderTotal,UseraccountId")] Furnitureorder furnitureorder)
        {
            if (ModelState.IsValid)
            {
                _context.Add(furnitureorder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UseraccountId"] = new SelectList(_context.Useraccounts, "Id", "Id", furnitureorder.UseraccountId);
            return View(furnitureorder);
        }

        // GET: Furnitureorders/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var furnitureorder = await _context.Furnitureorders.FindAsync(id);
            if (furnitureorder == null)
            {
                return NotFound();
            }
            ViewData["UseraccountId"] = new SelectList(_context.Useraccounts, "Id", "Id", furnitureorder.UseraccountId);
            return View(furnitureorder);
        }

        // POST: Furnitureorders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,OrderDate,OrderStatus,OrderTotal,UseraccountId")] Furnitureorder furnitureorder)
        {
            if (id != furnitureorder.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(furnitureorder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FurnitureorderExists(furnitureorder.Id))
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
            ViewData["UseraccountId"] = new SelectList(_context.Useraccounts, "Id", "Id", furnitureorder.UseraccountId);
            return View(furnitureorder);
        }

        // GET: Furnitureorders/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var furnitureorder = await _context.Furnitureorders
                .Include(f => f.Useraccount)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (furnitureorder == null)
            {
                return NotFound();
            }

            return View(furnitureorder);
        }

        // POST: Furnitureorders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            var furnitureorder = await _context.Furnitureorders.FindAsync(id);
            _context.Furnitureorders.Remove(furnitureorder);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FurnitureorderExists(decimal id)
        {
            return _context.Furnitureorders.Any(e => e.Id == id);
        }
    }
}
