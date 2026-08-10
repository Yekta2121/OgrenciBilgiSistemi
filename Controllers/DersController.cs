using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OgrenciBilgiSistemi.Data;
using OgrenciBilgiSistemi.Models;

namespace OgrenciBilgiSistemi.Controllers
{
    public class DersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Ders
        public async Task<IActionResult> Index()
        {
            var dersler = await _context.Dersler.ToListAsync();
            return View(dersler);
        }

        // GET: Ders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Ders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Ders ders)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ders);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ders);
        }

        // GET: Ders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var ders = await _context.Dersler.FindAsync(id);
            if (ders == null) return NotFound();

            return View(ders);
        }

        // POST: Ders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Ders ders)
        {
            if (id != ders.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ders);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Dersler.Any(e => e.Id == ders.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(ders);
        }

        // POST: Ders/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var ders = await _context.Dersler.FindAsync(id);
            if (ders != null)
            {
                _context.Dersler.Remove(ders);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}