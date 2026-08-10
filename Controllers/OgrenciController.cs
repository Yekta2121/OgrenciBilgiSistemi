using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OgrenciBilgiSistemi.Data;
using OgrenciBilgiSistemi.Models;

namespace OgrenciBilgiSistemi.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OgrenciController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OgrenciController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Ogrenci
        public async Task<IActionResult> Index()
        {
            var ogrenciler = await _context.Ogrenciler.ToListAsync();
            return View(ogrenciler);
        }

        // GET: Ogrenci/Create
        public IActionResult Create()
        {
            ViewBag.Bolumler = new SelectList(_context.Bolumler, "Id", "BolumAdi");
            return View();
        }

        // POST: Ogrenci/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Ogrenci ogrenci)
        {
            if (ModelState.IsValid)
            {
                ogrenci.OlusturmaTarihi = DateTime.UtcNow;
                _context.Add(ogrenci);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Bolumler = new SelectList(_context.Bolumler, "Id", "BolumAdi", ogrenci.BolumId);
            return View(ogrenci);
        }

        // GET: Ogrenci/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var ogrenci = await _context.Ogrenciler.FindAsync(id);
            if (ogrenci == null) return NotFound();

            ViewBag.Bolumler = new SelectList(_context.Bolumler, "Id", "BolumAdi", ogrenci.BolumId);
            return View(ogrenci);
        }

        // POST: Ogrenci/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Ogrenci ogrenci)
        {
            if (id != ogrenci.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ogrenci);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Ogrenciler.Any(e => e.Id == ogrenci.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Bolumler = new SelectList(_context.Bolumler, "Id", "BolumAdi", ogrenci.BolumId);
            return View(ogrenci);
        }

        // POST: Ogrenci/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var ogrenci = await _context.Ogrenciler.FindAsync(id);
            if (ogrenci != null)
            {
                _context.Ogrenciler.Remove(ogrenci);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}