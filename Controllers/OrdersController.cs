using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pedrito.Data;
using Pedrito.Models;

namespace Pedrito.Controllers
{
    public class OrdersController : Controller
    {
        private readonly PostgresDbContext _context;

        public OrdersController(PostgresDbContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var orders = await _context.Orders.Include(o => o.Client).ToListAsync();
            return View(orders);
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var order = await _context.Orders.Include(o => o.Client).FirstOrDefaultAsync(o => o.Id == id);
            if (order == null) return NotFound();
            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewBag.Clients = _context.Clients.ToList();
            return View();
        }

        // POST: Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderNumber,OrderDate,Status,ClientId")] Order order)
        {
            if (ModelState.IsValid)
            {
                order.OrderDate = DateTime.SpecifyKind(order.OrderDate, DateTimeKind.Utc);
                _context.Add(order);
                await _context.SaveChangesAsync();
                TempData["message"] = "Order created successfully.";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Clients = _context.Clients.ToList();
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return NotFound();
            ViewBag.Clients = _context.Clients.ToList();
            return View(order);
        }

        // POST: Orders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OrderNumber,OrderDate,Status,ClientId")] Order order)
        {
            if (id != order.Id) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    order.OrderDate = DateTime.SpecifyKind(order.OrderDate, DateTimeKind.Utc);
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                    TempData["message"] = "Order updated successfully.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Orders.Any(e => e.Id == order.Id))
                        return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Clients = _context.Clients.ToList();
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var order = await _context.Orders.Include(o => o.Client).FirstOrDefaultAsync(o => o.Id == id);
            if (order == null) return NotFound();
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
                TempData["message"] = "Order deleted successfully.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
