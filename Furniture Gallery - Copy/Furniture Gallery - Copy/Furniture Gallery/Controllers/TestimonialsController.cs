using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Furniture_Gallery.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Specialized;

namespace Furniture_Gallery.Controllers
{
    public class TestimonialsController : Controller
    {
        private readonly ModelContext _context;

        public object Entity { get; private set; }

        public TestimonialsController(ModelContext context)
        {
            _context = context;
        }

        // GET: Testimonials
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Testimonials.Include(t => t.Useraccount);
            return View(await modelContext.ToListAsync());
        }

        // GET: Testimonials/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testimonial = await _context.Testimonials
                .Include(t => t.Useraccount)
                .FirstOrDefaultAsync(m => m.TestimonialId == id);
            if (testimonial == null)
            {
                return NotFound();
            }

            return View(testimonial);
        }

        // GET: Testimonials/Create
        public IActionResult Create()
        {    
            ViewData["UseraccountId"] = new SelectList(_context.Useraccounts, "Id", "Id");
            return View();
        }

        // POST: Testimonials/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TestimonialId,UseraccountId,Rating,Message,TestimonialStatus")] Testimonial testimonial /*string Massage*/)
        {
            if (ModelState.IsValid)
            {
                //testimonial.Message = Massage;
                _context.Add(testimonial);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UseraccountId"] = new SelectList(_context.Useraccounts, "Id", "Id", testimonial.UseraccountId);
            return View(testimonial);
        }

        // GET: Testimonials/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testimonial = await _context.Testimonials.FindAsync(id);
            if (testimonial == null)
            {
                return NotFound();
            }
            ViewData["UseraccountId"] = new SelectList(_context.Useraccounts, "Id", "Id", testimonial.UseraccountId);
            return View(testimonial);
        }

        // POST: Testimonials/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("TestimonialId,UseraccountId,Rating,Message,TestimonialStatus")] Testimonial testimonial)
        {
            if (id != testimonial.TestimonialId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(testimonial);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TestimonialExists(testimonial.TestimonialId))
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
            ViewData["UseraccountId"] = new SelectList(_context.Useraccounts, "Id", "Id", testimonial.UseraccountId);
            return View(testimonial);
        }

        // GET: Testimonials/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testimonial = await _context.Testimonials
                .Include(t => t.Useraccount)
                .FirstOrDefaultAsync(m => m.TestimonialId == id);
            if (testimonial == null)
            {
                return NotFound();
            }

            return View(testimonial);
        }

        // POST: Testimonials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            var testimonial = await _context.Testimonials.FindAsync(id);
            _context.Testimonials.Remove(testimonial);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TestimonialExists(decimal id)
        {
            return _context.Testimonials.Any(e => e.TestimonialId == id);
        }

        

        public IActionResult Addreview()
        {
            ViewData["UseraccountId"] = new SelectList(_context.Useraccounts, "Id", "Id");
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Addreview(string Massage)
        {

            decimal userId = Convert.ToDecimal(HttpContext.Session.GetInt32("IdCustomer"));
            // check if user is logged in
            if (userId != null)
            {
                Testimonial testimonial = new Testimonial();

                testimonial.UseraccountId = userId;
                testimonial.TestimonialStatus = "pending";
                 testimonial.Message = Massage;

                _context.Add(testimonial);
                await _context.SaveChangesAsync();




                // redirect user to a success page
                return RedirectToAction("GetProduct","Home");
            }
            else
            {
                // redirect user to login page
                return RedirectToAction("Login", "RegisterLogin");
            }
        }







        [HttpPost]
        public ActionResult ApproveTestimonial(decimal testimonialId)
        {
            var testimonial = _context.Testimonials.Find(testimonialId);
            if (testimonial != null )
            {
                testimonial.TestimonialStatus = "Approved";
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult DisapproveTestimonial(int testimonialId)
        {
            var testimonial = _context.Testimonials.Find(testimonialId);
            if (testimonial != null)
            {
                testimonial.TestimonialStatus = "Disapproved";
               _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }



    }
}