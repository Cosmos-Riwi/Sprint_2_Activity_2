using System;
using System.Linq;
using System.Threading.Tasks;
using GestionRestaurante.Data;
using GestionRestaurante.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GestionRestaurante.Controllers
{
    public class PedidosController(ApplicationDbContext context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var pedidos = await context.Pedidos
                .Include(p => p.Cliente)
                .AsNoTracking()
                .ToListAsync();
            return View(pedidos);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var pedido = await context.Pedidos
                .Include(p => p.Cliente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pedido == null) return NotFound();
            return View(pedido);
        }

        public async Task<IActionResult> Create()
        {
            await CargarClientes();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Pedido pedido)
        {
            await CargarClientes();
            if (!ModelState.IsValid) return View(pedido);
            context.Add(pedido);
            await context.SaveChangesAsync();
            TempData["Success"] = "Pedido creado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var pedido = await context.Pedidos.FindAsync(id);
            if (pedido == null) return NotFound();
            await CargarClientes();
            return View(pedido);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Pedido pedido)
        {
            if (id != pedido.Id) return NotFound();
            await CargarClientes();
            if (!ModelState.IsValid) return View(pedido);
            context.Update(pedido);
            await context.SaveChangesAsync();
            TempData["Success"] = "Pedido actualizado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var pedido = await context.Pedidos.Include(p => p.Cliente).FirstOrDefaultAsync(m => m.Id == id);
            if (pedido == null) return NotFound();
            return View(pedido);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pedido = await context.Pedidos.FindAsync(id);
            if (pedido == null) return NotFound();
            context.Pedidos.Remove(pedido);
            await context.SaveChangesAsync();
            TempData["Success"] = "Pedido eliminado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        private async Task CargarClientes()
        {
            var clientes = await context.Clientes
                .AsNoTracking()
                .Select(c => new { c.Id, Nombre = c.nombre + " " + c.apellido })
                .ToListAsync();
            ViewData["ClienteId"] = new SelectList(clientes, "Id", "Nombre");
        }
    }
}


