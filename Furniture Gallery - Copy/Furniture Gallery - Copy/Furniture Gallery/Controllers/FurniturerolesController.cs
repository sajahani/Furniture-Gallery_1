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
    public class FurniturerolesController : Controller
    {
        private readonly ModelContext _context;

        public FurniturerolesController(ModelContext context)
        {
            _context = context;
        }

        // GET: Furnitureroles
        public async Task<IActionResult> Index()
        {
            return View(await _context.Furnitureroles.ToListAsync());
        }

        // GET: Furnitureroles/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var furniturerole = await _context.Furnitureroles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (furniturerole == null)
            {
                return NotFound();
            }

            return View(furniturerole);
        }

        // GET: Furnitureroles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Furnitureroles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Rolename")] Furniturerole furniturerole)
        {
            if (ModelState.IsValid)
            {
                _context.Add(furniturerole);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(furniturerole);
        }

        // GET: Furnitureroles/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var furniturerole = await _context.Furnitureroles.FindAsync(id);
            if (furniturerole == null)
            {
                return NotFound();
            }
            return View(furniturerole);
        }

        // POST: Furnitureroles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,Rolename")] Furniturerole furniturerole)
        {
            if (id != furniturerole.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(furniturerole);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FurnitureroleExists(furniturerole.Id))
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
            return View(furniturerole);
        }

        // GET: Furnitureroles/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var furniturerole = await _context.Furnitureroles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (furniturerole == null)
            {
                return NotFound();
            }

            return View(furniturerole);
        }

        // POST: Furnitureroles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            var furniturerole = await _context.Furnitureroles.FindAsync(id);
            _context.Furnitureroles.Remove(furniturerole);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FurnitureroleExists(decimal id)
        {
            return _context.Furnitureroles.Any(e => e.Id == id);
        }
    }
}
