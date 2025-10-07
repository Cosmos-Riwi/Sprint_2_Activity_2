using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pedrito.Data;
using Pedrito.Models;

namespace Pedrito.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly PostgresDbContext _context;

        public ReservationsController(PostgresDbContext context)
        {
            _context = context;
        }

        // GET: Reservations
        public async Task<IActionResult> Index()
        {
            var reservations = await _context.Reservations.Include(r => r.Client).ToListAsync();
            return View(reservations);
        }

        // GET: Reservations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var reservation = await _context.Reservations.Include(r => r.Client).FirstOrDefaultAsync(r => r.Id == id);
            if (reservation == null) return NotFound();
            return View(reservation);
        }

        // GET: Reservations/Create
        public IActionResult Create()
        {
            ViewBag.Clients = _context.Clients.ToList();
            return View();
        }

        // POST: Reservations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReservationDate,ReservationTime,PartySize,Notes,ClientId")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                reservation.ReservationDate = DateTime.SpecifyKind(reservation.ReservationDate, DateTimeKind.Utc);
                _context.Add(reservation);
                await _context.SaveChangesAsync();
                TempData["message"] = "Reservation created successfully.";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Clients = _context.Clients.ToList();
            return View(reservation);
        }

        // GET: Reservations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null) return NotFound();
            ViewBag.Clients = _context.Clients.ToList();
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ReservationDate,ReservationTime,PartySize,Notes,ClientId")] Reservation reservation)
        {
            if (id != reservation.Id) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    reservation.ReservationDate = DateTime.SpecifyKind(reservation.ReservationDate, DateTimeKind.Utc);
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                    TempData["message"] = "Reservation updated successfully.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Reservations.Any(e => e.Id == reservation.Id))
                        return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Clients = _context.Clients.ToList();
            return View(reservation);
        }

        // GET: Reservations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var reservation = await _context.Reservations.Include(r => r.Client).FirstOrDefaultAsync(r => r.Id == id);
            if (reservation == null) return NotFound();
            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation != null)
            {
                _context.Reservations.Remove(reservation);
                await _context.SaveChangesAsync();
                TempData["message"] = "Reservation deleted successfully.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
