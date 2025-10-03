using Microsoft.AspNetCore.Mvc;
using RestaurantSystem.Services;
using RestaurantSystem.Models;

namespace RestaurantSystem.Controllers
{
    /// <summary>
    /// Controller for waiter management operations
    /// </summary>
    public class WaiterController : Controller
    {
        private readonly WaiterService _waiterService;

        public WaiterController(WaiterService waiterService)
        {
            _waiterService = waiterService;
        }

        /// <summary>
        /// Display list of waiters
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var waiters = await _waiterService.GetAllAsync();
            ViewData["Title"] = "Gestión de Meseros";
            return View(waiters);
        }

        /// <summary>
        /// Show waiter details
        /// </summary>
        public async Task<IActionResult> Details(int id)
        {
            var waiter = await _waiterService.GetByIdAsync(id);
            if (waiter == null)
            {
                return NotFound();
            }
            ViewData["Title"] = $"Detalles - {waiter.FullName}";
            return View(waiter);
        }

        /// <summary>
        /// Show create waiter form
        /// </summary>
        public IActionResult Create()
        {
            ViewData["Title"] = "Crear Mesero";
            return View();
        }

        /// <summary>
        /// Process waiter creation
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Waiter waiter)
        {
            if (ModelState.IsValid)
            {
                var result = await _waiterService.CreateAsync(waiter);
                if (result.IsSuccess)
                {
                    TempData["SuccessMessage"] = result.Message;
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", result.Message);
            }
            ViewData["Title"] = "Crear Mesero";
            return View(waiter);
        }

        /// <summary>
        /// Show edit waiter form
        /// </summary>
        public async Task<IActionResult> Edit(int id)
        {
            var waiter = await _waiterService.GetByIdAsync(id);
            if (waiter == null)
            {
                return NotFound();
            }
            ViewData["Title"] = $"Editar - {waiter.FullName}";
            return View(waiter);
        }

        /// <summary>
        /// Process waiter update
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Waiter waiter)
        {
            if (id != waiter.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var result = await _waiterService.UpdateAsync(waiter);
                if (result.IsSuccess)
                {
                    TempData["SuccessMessage"] = result.Message;
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", result.Message);
            }
            ViewData["Title"] = $"Editar - {waiter.FullName}";
            return View(waiter);
        }

        /// <summary>
        /// Show delete confirmation
        /// </summary>
        public async Task<IActionResult> Delete(int id)
        {
            var waiter = await _waiterService.GetByIdAsync(id);
            if (waiter == null)
            {
                return NotFound();
            }
            ViewData["Title"] = $"Eliminar - {waiter.FullName}";
            return View(waiter);
        }

        /// <summary>
        /// Process waiter deletion
        /// </summary>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _waiterService.DeleteAsync(id);
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
