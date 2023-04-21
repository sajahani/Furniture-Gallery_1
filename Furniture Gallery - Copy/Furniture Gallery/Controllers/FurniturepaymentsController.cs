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
    public class FurniturepaymentsController : Controller
    {
        private readonly ModelContext _context;

        public FurniturepaymentsController(ModelContext context)
        {
            _context = context;
        }

        // GET: Furniturepayments
        public async Task<IActionResult> Index()
        {
            return View(await _context.Furniturepayments.ToListAsync());
        }

        // GET: Furniturepayments/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var furniturepayment = await _context.Furniturepayments
                .FirstOrDefaultAsync(m => m.Id == id);
            if (furniturepayment == null)
            {
                return NotFound();
            }

            return View(furniturepayment);
        }

        // GET: Furniturepayments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Furniturepayments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PaymentDate,PaymentAmount")] Furniturepayment furniturepayment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(furniturepayment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(furniturepayment);
        }

        // GET: Furniturepayments/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var furniturepayment = await _context.Furniturepayments.FindAsync(id);
            if (furniturepayment == null)
            {
                return NotFound();
            }
            return View(furniturepayment);
        }

        // POST: Furniturepayments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,PaymentDate,PaymentAmount")] Furniturepayment furniturepayment)
        {
            if (id != furniturepayment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(furniturepayment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FurniturepaymentExists(furniturepayment.Id))
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
            return View(furniturepayment);
        }

        // GET: Furniturepayments/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var furniturepayment = await _context.Furniturepayments
                .FirstOrDefaultAsync(m => m.Id == id);
            if (furniturepayment == null)
            {
                return NotFound();
            }

            return View(furniturepayment);
        }

        // POST: Furniturepayments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            var furniturepayment = await _context.Furniturepayments.FindAsync(id);
            _context.Furniturepayments.Remove(furniturepayment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FurniturepaymentExists(decimal id)
        {
            return _context.Furniturepayments.Any(e => e.Id == id);
        }
    }
}
