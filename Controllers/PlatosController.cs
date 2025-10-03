using System;
using System.Linq;
using System.Threading.Tasks;
using GestionRestaurante.Data;
using GestionRestaurante.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestionRestaurante.Controllers
{
    public class PlatosController(ApplicationDbContext context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var platos = await context.Platos.AsNoTracking().ToListAsync();
            return View(platos);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var plato = await context.Platos.FirstOrDefaultAsync(m => m.Id == id);
            if (plato == null) return NotFound();
            return View(plato);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Plato plato)
        {
            if (!ModelState.IsValid) return View(plato);
            if (plato.precio <= 0) ModelState.AddModelError(nameof(Plato.precio), "El precio debe ser mayor a 0.");
            if (!ModelState.IsValid) return View(plato);
            context.Add(plato);
            await context.SaveChangesAsync();
            TempData["Success"] = "Plato creado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var plato = await context.Platos.FindAsync(id);
            if (plato == null) return NotFound();
            return View(plato);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Plato plato)
        {
            if (id != plato.Id) return NotFound();
            if (plato.precio <= 0) ModelState.AddModelError(nameof(Plato.precio), "El precio debe ser mayor a 0.");
            if (!ModelState.IsValid) return View(plato);
            context.Update(plato);
            await context.SaveChangesAsync();
            TempData["Success"] = "Plato actualizado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var plato = await context.Platos.FirstOrDefaultAsync(m => m.Id == id);
            if (plato == null) return NotFound();
            return View(plato);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var plato = await context.Platos.FindAsync(id);
            if (plato == null) return NotFound();
            context.Platos.Remove(plato);
            await context.SaveChangesAsync();
            TempData["Success"] = "Plato eliminado correctamente.";
            return RedirectToAction(nameof(Index));
        }
    }
}


