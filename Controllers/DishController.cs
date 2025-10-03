using Microsoft.AspNetCore.Mvc;
using RestaurantSystem.Services;
using RestaurantSystem.Models;

namespace RestaurantSystem.Controllers
{
    /// <summary>
    /// Controller for dish management operations
    /// </summary>
    public class DishController : Controller
    {
        private readonly DishService _dishService;

        public DishController(DishService dishService)
        {
            _dishService = dishService;
        }

        /// <summary>
        /// Display list of dishes
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var dishes = await _dishService.GetAllAsync();
            ViewData["Title"] = "Gestión de Platos";
            return View(dishes);
        }

        /// <summary>
        /// Show dish details
        /// </summary>
        public async Task<IActionResult> Details(int id)
        {
            var dish = await _dishService.GetByIdAsync(id);
            if (dish == null)
            {
                return NotFound();
            }
            ViewData["Title"] = $"Detalles - {dish.Name}";
            return View(dish);
        }

        /// <summary>
        /// Show create dish form
        /// </summary>
        public IActionResult Create()
        {
            ViewData["Title"] = "Crear Plato";
            return View();
        }

        /// <summary>
        /// Process dish creation
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Dish dish)
        {
            if (ModelState.IsValid)
            {
                var result = await _dishService.CreateAsync(dish);
                if (result.IsSuccess)
                {
                    TempData["SuccessMessage"] = result.Message;
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", result.Message);
            }
            ViewData["Title"] = "Crear Plato";
            return View(dish);
        }

        /// <summary>
        /// Show edit dish form
        /// </summary>
        public async Task<IActionResult> Edit(int id)
        {
            var dish = await _dishService.GetByIdAsync(id);
            if (dish == null)
            {
                return NotFound();
            }
            ViewData["Title"] = $"Editar - {dish.Name}";
            return View(dish);
        }

        /// <summary>
        /// Process dish update
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Dish dish)
        {
            if (id != dish.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var result = await _dishService.UpdateAsync(dish);
                if (result.IsSuccess)
                {
                    TempData["SuccessMessage"] = result.Message;
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", result.Message);
            }
            ViewData["Title"] = $"Editar - {dish.Name}";
            return View(dish);
        }

        /// <summary>
        /// Show delete confirmation
        /// </summary>
        public async Task<IActionResult> Delete(int id)
        {
            var dish = await _dishService.GetByIdAsync(id);
            if (dish == null)
            {
                return NotFound();
            }
            ViewData["Title"] = $"Eliminar - {dish.Name}";
            return View(dish);
        }

        /// <summary>
        /// Process dish deletion
        /// </summary>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _dishService.DeleteAsync(id);
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
