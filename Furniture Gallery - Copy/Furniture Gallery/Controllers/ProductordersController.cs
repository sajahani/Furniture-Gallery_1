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
    public class ProductordersController : Controller
    {
        private readonly ModelContext _context;

        public ProductordersController(ModelContext context)
        {
            _context = context;
        }

        // GET: Productorders
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Productorders.Include(p => p.Order).Include(p => p.Payment).Include(p => p.Product);
            return View(await modelContext.ToListAsync());
        }

        // GET: Productorders/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productorder = await _context.Productorders
                .Include(p => p.Order)
                .Include(p => p.Payment)
                .Include(p => p.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productorder == null)
            {
                return NotFound();
            }

            return View(productorder);
        }

        // GET: Productorders/Create
        public IActionResult Create()
        {
            ViewData["OrderId"] = new SelectList(_context.Furnitureorders, "Id", "Id");
            ViewData["PaymentId"] = new SelectList(_context.Furniturepayments, "Id", "Id");
            ViewData["ProductId"] = new SelectList(_context.Furnitureproducts, "Id", "Productname");
            return View();
        }

        // POST: Productorders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,OrderId,ProductId,Quantity,PaymentId")] Productorder productorder ,decimal qty)
        {
            if (ModelState.IsValid)
            {
                productorder.Quantity = qty;
                _context.Add(productorder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OrderId"] = new SelectList(_context.Furnitureorders, "Id", "Id", productorder.OrderId);
            ViewData["PaymentId"] = new SelectList(_context.Furniturepayments, "Id", "Id", productorder.PaymentId);
            ViewData["ProductId"] = new SelectList(_context.Furnitureproducts, "Id", "Productname", productorder.ProductId);
            return View(productorder);
        }

        // GET: Productorders/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productorder = await _context.Productorders.FindAsync(id);
            if (productorder == null)
            {
                return NotFound();
            }
            ViewData["OrderId"] = new SelectList(_context.Furnitureorders, "Id", "Id", productorder.OrderId);
            ViewData["PaymentId"] = new SelectList(_context.Furniturepayments, "Id", "Id", productorder.PaymentId);
            ViewData["ProductId"] = new SelectList(_context.Furnitureproducts, "Id", "Productname", productorder.ProductId);
            return View(productorder);
        }

        // POST: Productorders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,OrderId,ProductId,Quantity,PaymentId")] Productorder productorder)
        {
            if (id != productorder.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productorder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductorderExists(productorder.Id))
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
            ViewData["OrderId"] = new SelectList(_context.Furnitureorders, "Id", "Id", productorder.OrderId);
            ViewData["PaymentId"] = new SelectList(_context.Furniturepayments, "Id", "Id", productorder.PaymentId);
            ViewData["ProductId"] = new SelectList(_context.Furnitureproducts, "Id", "Productname", productorder.ProductId);
            return View(productorder);
        }

        // GET: Productorders/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productorder = await _context.Productorders
                .Include(p => p.Order)
                .Include(p => p.Payment)
                .Include(p => p.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productorder == null)
            {
                return NotFound();
            }

            return View(productorder);
        }

        // POST: Productorders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            var productorder = await _context.Productorders.FindAsync(id);
            _context.Productorders.Remove(productorder);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductorderExists(decimal id)
        {
            return _context.Productorders.Any(e => e.Id == id);
        }
    }
}
