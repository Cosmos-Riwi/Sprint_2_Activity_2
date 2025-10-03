using Microsoft.AspNetCore.Mvc;
using RestaurantSystem.Services;
using RestaurantSystem.Models;

namespace RestaurantSystem.Controllers
{
    /// <summary>
    /// Controller for order management operations
    /// </summary>
    public class OrderController : Controller
    {
        private readonly OrderService _orderService;
        private readonly CustomerService _customerService;

        public OrderController(OrderService orderService, CustomerService customerService)
        {
            _orderService = orderService;
            _customerService = customerService;
        }

        /// <summary>
        /// Display list of orders
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var orders = await _orderService.GetAllAsync();
            ViewData["Title"] = "Gestión de Pedidos";
            return View(orders);
        }

        /// <summary>
        /// Show order details
        /// </summary>
        public async Task<IActionResult> Details(int id)
        {
            var order = await _orderService.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["Title"] = $"Detalles - Pedido #{order.OrderNumber}";
            return View(order);
        }

        /// <summary>
        /// Show create order form
        /// </summary>
        public async Task<IActionResult> Create()
        {
            ViewData["Title"] = "Crear Pedido";
            ViewBag.Customers = await _customerService.GetAllAsync();
            return View();
        }

        /// <summary>
        /// Process order creation
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Order order)
        {
            if (ModelState.IsValid)
            {
                var result = await _orderService.CreateAsync(order);
                if (result.IsSuccess)
                {
                    TempData["SuccessMessage"] = result.Message;
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", result.Message);
            }
            ViewData["Title"] = "Crear Pedido";
            ViewBag.Customers = await _customerService.GetAllAsync();
            return View(order);
        }

        /// <summary>
        /// Show edit order form
        /// </summary>
        public async Task<IActionResult> Edit(int id)
        {
            var order = await _orderService.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["Title"] = $"Editar - Pedido #{order.OrderNumber}";
            ViewBag.Customers = await _customerService.GetAllAsync();
            return View(order);
        }

        /// <summary>
        /// Process order update
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var result = await _orderService.UpdateAsync(order);
                if (result.IsSuccess)
                {
                    TempData["SuccessMessage"] = result.Message;
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", result.Message);
            }
            ViewData["Title"] = $"Editar - Pedido #{order.OrderNumber}";
            ViewBag.Customers = await _customerService.GetAllAsync();
            return View(order);
        }

        /// <summary>
        /// Show delete confirmation
        /// </summary>
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _orderService.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["Title"] = $"Eliminar - Pedido #{order.OrderNumber}";
            return View(order);
        }

        /// <summary>
        /// Process order deletion
        /// </summary>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _orderService.DeleteAsync(id);
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
