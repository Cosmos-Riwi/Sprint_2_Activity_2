using Microsoft.AspNetCore.Mvc;
using RestaurantSystem.Services;
using RestaurantSystem.Models;

namespace RestaurantSystem.Controllers
{
    /// <summary>
    /// Controller for reservation management operations
    /// </summary>
    public class ReservationController : Controller
    {
        private readonly ReservationService _reservationService;
        private readonly CustomerService _customerService;

        public ReservationController(ReservationService reservationService, CustomerService customerService)
        {
            _reservationService = reservationService;
            _customerService = customerService;
        }

        /// <summary>
        /// Display list of reservations
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var reservations = await _reservationService.GetAllAsync();
            ViewData["Title"] = "Gestión de Reservas";
            return View(reservations);
        }

        /// <summary>
        /// Show reservation details
        /// </summary>
        public async Task<IActionResult> Details(int id)
        {
            var reservation = await _reservationService.GetByIdAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }
            ViewData["Title"] = $"Detalles - Reserva #{reservation.Id}";
            return View(reservation);
        }

        /// <summary>
        /// Show create reservation form
        /// </summary>
        public async Task<IActionResult> Create()
        {
            ViewData["Title"] = "Crear Reserva";
            ViewBag.Customers = await _customerService.GetAllAsync();
            return View();
        }

        /// <summary>
        /// Process reservation creation
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                var result = await _reservationService.CreateAsync(reservation);
                if (result.IsSuccess)
                {
                    TempData["SuccessMessage"] = result.Message;
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", result.Message);
            }
            ViewData["Title"] = "Crear Reserva";
            ViewBag.Customers = await _customerService.GetAllAsync();
            return View(reservation);
        }

        /// <summary>
        /// Show edit reservation form
        /// </summary>
        public async Task<IActionResult> Edit(int id)
        {
            var reservation = await _reservationService.GetByIdAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }
            ViewData["Title"] = $"Editar - Reserva #{reservation.Id}";
            ViewBag.Customers = await _customerService.GetAllAsync();
            return View(reservation);
        }

        /// <summary>
        /// Process reservation update
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Reservation reservation)
        {
            if (id != reservation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var result = await _reservationService.UpdateAsync(reservation);
                if (result.IsSuccess)
                {
                    TempData["SuccessMessage"] = result.Message;
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", result.Message);
            }
            ViewData["Title"] = $"Editar - Reserva #{reservation.Id}";
            ViewBag.Customers = await _customerService.GetAllAsync();
            return View(reservation);
        }

        /// <summary>
        /// Show delete confirmation
        /// </summary>
        public async Task<IActionResult> Delete(int id)
        {
            var reservation = await _reservationService.GetByIdAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }
            ViewData["Title"] = $"Eliminar - Reserva #{reservation.Id}";
            return View(reservation);
        }

        /// <summary>
        /// Process reservation deletion
        /// </summary>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _reservationService.DeleteAsync(id);
            if (result.IsSuccess)
            {
                TempData["SuccessMessage"] = result.Message;
            }
            else
            {
                TempData["ErrorMessage"] = result.Message;
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
