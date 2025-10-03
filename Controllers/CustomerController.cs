using Microsoft.AspNetCore.Mvc;
using RestaurantSystem.Services;
using RestaurantSystem.Models;

namespace RestaurantSystem.Controllers
{
    /// <summary>
    /// Controller for customer management operations
    /// </summary>
    public class CustomerController : Controller
    {
        private readonly CustomerService _customerService;

        public CustomerController(CustomerService customerService)
        {
            _customerService = customerService;
        }

        /// <summary>
        /// Display list of customers
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var customers = await _customerService.GetAllAsync();
            ViewData["Title"] = "Gestión de Clientes";
            return View(customers);
        }

        /// <summary>
        /// Show customer details
        /// </summary>
        public async Task<IActionResult> Details(int id)
        {
            var customer = await _customerService.GetByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            ViewData["Title"] = $"Detalles - {customer.FullName}";
            return View(customer);
        }

        /// <summary>
        /// Show create customer form
        /// </summary>
        public IActionResult Create()
        {
            ViewData["Title"] = "Crear Cliente";
            return View();
        }

        /// <summary>
        /// Process customer creation
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Customer customer)
        {
            if (ModelState.IsValid)
            {
                var result = await _customerService.CreateAsync(customer);
                if (result.IsSuccess)
                {
                    TempData["SuccessMessage"] = result.Message;
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", result.Message);
            }
            ViewData["Title"] = "Crear Cliente";
            return View(customer);
        }

        /// <summary>
        /// Show edit customer form
        /// </summary>
        public async Task<IActionResult> Edit(int id)
        {
            var customer = await _customerService.GetByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            ViewData["Title"] = $"Editar - {customer.FullName}";
            return View(customer);
        }

        /// <summary>
        /// Process customer update
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Customer customer)
        {
            if (id != customer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var result = await _customerService.UpdateAsync(customer);
                if (result.IsSuccess)
                {
                    TempData["SuccessMessage"] = result.Message;
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", result.Message);
            }
            ViewData["Title"] = $"Editar - {customer.FullName}";
            return View(customer);
        }

        /// <summary>
        /// Show delete confirmation
        /// </summary>
        public async Task<IActionResult> Delete(int id)
        {
            var customer = await _customerService.GetByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            ViewData["Title"] = $"Eliminar - {customer.FullName}";
            return View(customer);
        }

        /// <summary>
        /// Process customer deletion
        /// </summary>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _customerService.DeleteAsync(id);
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
