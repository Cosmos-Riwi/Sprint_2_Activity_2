using System.Linq;
using System.Threading.Tasks;
using GestionRestaurante.Data;
using GestionRestaurante.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestionRestaurante.Controllers
{
    public class MeserosController(ApplicationDbContext context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var meseros = await context.Meseros.AsNoTracking().ToListAsync();
            return View(meseros);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var mesero = await context.Meseros.FirstOrDefaultAsync(m => m.Id == id);
            if (mesero == null) return NotFound();
            return View(mesero);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Mesero mesero)
        {
            if (!ModelState.IsValid) return View(mesero);
            context.Add(mesero);
            await context.SaveChangesAsync();
            TempData["Success"] = "Mesero creado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var mesero = await context.Meseros.FindAsync(id);
            if (mesero == null) return NotFound();
            return View(mesero);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Mesero mesero)
        {
            if (id != mesero.Id) return NotFound();
            if (!ModelState.IsValid) return View(mesero);
            context.Update(mesero);
            await context.SaveChangesAsync();
            TempData["Success"] = "Mesero actualizado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var mesero = await context.Meseros.FirstOrDefaultAsync(m => m.Id == id);
            if (mesero == null) return NotFound();
            return View(mesero);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mesero = await context.Meseros.FindAsync(id);
            if (mesero == null) return NotFound();
            context.Meseros.Remove(mesero);
            await context.SaveChangesAsync();
            TempData["Success"] = "Mesero eliminado correctamente.";
            return RedirectToAction(nameof(Index));
        }
    }
}


