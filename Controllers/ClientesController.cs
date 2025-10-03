using System.Linq;
using System.Threading.Tasks;
using GestionRestaurante.Data;
using GestionRestaurante.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestionRestaurante.Controllers
{
    public class ClientesController(ApplicationDbContext context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var clientes = await context.Clientes
                .AsNoTracking()
                .ToListAsync();
            return View(clientes);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var cliente = await context.Clientes
                .Include(c => c.Pedidos)
                .Include(c => c.Reservas)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cliente == null) return NotFound();
            return View(cliente);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cliente cliente)
        {
            if (!ModelState.IsValid) return View(cliente);
            context.Add(cliente);
            await context.SaveChangesAsync();
            TempData["Success"] = "Cliente creado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var cliente = await context.Clientes.FindAsync(id);
            if (cliente == null) return NotFound();
            return View(cliente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Cliente cliente)
        {
            if (id != cliente.Id) return NotFound();
            if (!ModelState.IsValid) return View(cliente);
            try
            {
                context.Update(cliente);
                await context.SaveChangesAsync();
                TempData["Success"] = "Cliente actualizado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!context.Clientes.Any(e => e.Id == id)) return NotFound();
                throw;
            }
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var cliente = await context.Clientes
                .Include(c => c.Pedidos)
                .Include(c => c.Reservas)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cliente == null) return NotFound();
            return View(cliente);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cliente = await context.Clientes.FindAsync(id);
            if (cliente == null) return NotFound();
            context.Clientes.Remove(cliente);
            await context.SaveChangesAsync();
            TempData["Success"] = "Cliente eliminado correctamente.";
            return RedirectToAction(nameof(Index));
        }
    }
}


