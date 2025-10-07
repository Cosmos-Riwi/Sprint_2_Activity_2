using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pedrito.Data;
using Pedrito.Models;

namespace Pedrito.Controllers
{
    public class WaitersController : Controller
    {
        private readonly PostgresDbContext _context;

        public WaitersController(PostgresDbContext context)
        {
            _context = context;
        }

        // GET: Waiters
        public async Task<IActionResult> Index()
        {
            var waiters = await _context.Waiters.ToListAsync();
            return View(waiters);
        }

        // GET: Waiters/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var waiter = await _context.Waiters.FindAsync(id);
            if (waiter == null) return NotFound();
            return View(waiter);
        }

        // GET: Waiters/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Waiters/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,Shift,YearsExperience")] Waiter waiter)
        {
            if (ModelState.IsValid)
            {
                _context.Add(waiter);
                await _context.SaveChangesAsync();
                TempData["message"] = "Waiter created successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View(waiter);
        }

        // GET: Waiters/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var waiter = await _context.Waiters.FindAsync(id);
            if (waiter == null) return NotFound();
            return View(waiter);
        }

        // POST: Waiters/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Shift,YearsExperience")] Waiter waiter)
        {
            if (id != waiter.Id) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(waiter);
                    await _context.SaveChangesAsync();
                    TempData["message"] = "Waiter updated successfully.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Waiters.Any(e => e.Id == waiter.Id))
                        return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(waiter);
        }

        // GET: Waiters/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var waiter = await _context.Waiters.FindAsync(id);
            if (waiter == null) return NotFound();
            return View(waiter);
        }

        // POST: Waiters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var waiter = await _context.Waiters.FindAsync(id);
            if (waiter != null)
            {
                _context.Waiters.Remove(waiter);
                await _context.SaveChangesAsync();
                TempData["message"] = "Waiter deleted successfully.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
