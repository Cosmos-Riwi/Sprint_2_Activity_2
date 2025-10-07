using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pedrito.Data;
using Pedrito.Models;

namespace Pedrito.Controllers
{
    public class DishesController : Controller
    {
        private readonly PostgresDbContext _context;

        public DishesController(PostgresDbContext context)
        {
            _context = context;
        }

        // GET: Dishes
        public async Task<IActionResult> Index()
        {
            var dishes = await _context.Dishes.ToListAsync();
            return View(dishes);
        }

        // GET: Dishes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var dish = await _context.Dishes.FindAsync(id);
            if (dish == null) return NotFound();
            return View(dish);
        }

        // GET: Dishes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Dishes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,Price,Category")] Dish dish)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dish);
                await _context.SaveChangesAsync();
                TempData["message"] = "Dish created successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View(dish);
        }

        // GET: Dishes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var dish = await _context.Dishes.FindAsync(id);
            if (dish == null) return NotFound();
            return View(dish);
        }

        // POST: Dishes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price,Category")] Dish dish)
        {
            if (id != dish.Id) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dish);
                    await _context.SaveChangesAsync();
                    TempData["message"] = "Dish updated successfully.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Dishes.Any(e => e.Id == dish.Id))
                        return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(dish);
        }

        // GET: Dishes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var dish = await _context.Dishes.FindAsync(id);
            if (dish == null) return NotFound();
            return View(dish);
        }

        // POST: Dishes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dish = await _context.Dishes.FindAsync(id);
            if (dish != null)
            {
                _context.Dishes.Remove(dish);
                await _context.SaveChangesAsync();
                TempData["message"] = "Dish deleted successfully.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
