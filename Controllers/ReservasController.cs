using System.Linq;
using System.Threading.Tasks;
using GestionRestaurante.Data;
using GestionRestaurante.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GestionRestaurante.Controllers
{
    public class ReservasController(ApplicationDbContext context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var reservas = await context.Reservas.Include(r => r.Cliente).AsNoTracking().ToListAsync();
            return View(reservas);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var reserva = await context.Reservas.Include(r => r.Cliente).FirstOrDefaultAsync(m => m.Id == id);
            if (reserva == null) return NotFound();
            return View(reserva);
        }

        public async Task<IActionResult> Create()
        {
            await CargarClientes();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Reserva reserva)
        {
            await CargarClientes();
            if (!ModelState.IsValid) return View(reserva);
            context.Add(reserva);
            await context.SaveChangesAsync();
            TempData["Success"] = "Reserva creada correctamente.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var reserva = await context.Reservas.FindAsync(id);
            if (reserva == null) return NotFound();
            await CargarClientes();
            return View(reserva);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Reserva reserva)
        {
            if (id != reserva.Id) return NotFound();
            await CargarClientes();
            if (!ModelState.IsValid) return View(reserva);
            context.Update(reserva);
            await context.SaveChangesAsync();
            TempData["Success"] = "Reserva actualizada correctamente.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var reserva = await context.Reservas.Include(r => r.Cliente).FirstOrDefaultAsync(m => m.Id == id);
            if (reserva == null) return NotFound();
            return View(reserva);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reserva = await context.Reservas.FindAsync(id);
            if (reserva == null) return NotFound();
            context.Reservas.Remove(reserva);
            await context.SaveChangesAsync();
            TempData["Success"] = "Reserva eliminada correctamente.";
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


