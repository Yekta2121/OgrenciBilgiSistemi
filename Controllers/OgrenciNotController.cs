using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OgrenciBilgiSistemi.Data;
using OgrenciBilgiSistemi.Models;

namespace OgrenciBilgiSistemi.Controllers
{
    public class OgrenciNotController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OgrenciNotController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: OgrenciNot (Arama ve Filtreleme Destekli)
        public async Task<IActionResult> Index(string searchString, int? dersId)
        {
            var notlarQuery = _context.OgrenciNotlar
                .Include(o => o.Ogrenci)
                .Include(d => d.Ders)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                notlarQuery = notlarQuery.Where(n =>
                    n.Ogrenci!.Ad.Contains(searchString) ||
                    n.Ogrenci.Soyad.Contains(searchString) ||
                    n.Ogrenci.OgrenciNo.Contains(searchString));
            }

            if (dersId.HasValue && dersId.Value > 0)
            {
                notlarQuery = notlarQuery.Where(n => n.DersId == dersId.Value);
            }

            ViewBag.Dersler = new SelectList(await _context.Dersler.ToListAsync(), "Id", "DersAdi", dersId);
            ViewBag.CurrentSearch = searchString;

            return View(await notlarQuery.ToListAsync());
        }

        // GET: OgrenciNot/Transkript/5 (Öğrenci ID ye göre karne, AGNO ve Bölüm Bazlı Mezuniyet Kontrolü)
        public async Task<IActionResult> Transkript(int? id)
        {
            if (id == null) return NotFound();

            // Öğrenciyi ve bağlı olduğu Bölüm bilgilerini birlikte çekiyoruz
            var ogrenci = await _context.Ogrenciler
                .Include(o => o.Bolum)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (ogrenci == null) return NotFound();

            var notlar = await _context.OgrenciNotlar
                .Include(n => n.Ders)
                .Where(n => n.OgrenciId == id)
                .ToListAsync();

            ViewBag.Ogrenci = ogrenci;

            double toplamPuan = 0;
            int toplamKredi = 0;
            int basariliKredi = 0;

            foreach (var not in notlar)
            {
                int kredi = not.Ders?.YerelKredi ?? 3;
                toplamPuan += (not.HarfNotuKatsayisi * kredi);
                toplamKredi += kredi;

                if (not.GectiMi)
                {
                    basariliKredi += kredi;
                }
            }

            ViewBag.AGNO = toplamKredi > 0 ? Math.Round(toplamPuan / toplamKredi, 2) : 0.0;
            ViewBag.ToplamKredi = toplamKredi;
            ViewBag.BasariliKredi = basariliKredi;

            // Hedef kredi artık doğrudan bağlı olduğu Bölümün kuralından geliyor!
            int hedefKredi = ogrenci.Bolum?.MezuniyetIcinGerekliKredi ?? 120;
            ViewBag.MezuniyetIcinGerekliKredi = hedefKredi;
            ViewBag.MezunOlduMu = basariliKredi >= hedefKredi;

            return View(notlar);
        }

        // GET: OgrenciNot/Create
        public async Task<IActionResult> Create()
        {
            await PopulateDropdownsAsync();
            return View();
        }

        // POST: OgrenciNot/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OgrenciNot ogrenciNot)
        {
            if (ModelState.IsValid)
            {
                HesaplaVeAtamaYap(ogrenciNot);
                _context.Add(ogrenciNot);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            await PopulateDropdownsAsync(ogrenciNot.OgrenciId, ogrenciNot.DersId);
            return View(ogrenciNot);
        }

        // GET: OgrenciNot/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var ogrenciNot = await _context.OgrenciNotlar.FindAsync(id);
            if (ogrenciNot == null) return NotFound();

            await PopulateDropdownsAsync(ogrenciNot.OgrenciId, ogrenciNot.DersId);
            return View(ogrenciNot);
        }

        // POST: OgrenciNot/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, OgrenciNot ogrenciNot)
        {
            if (id != ogrenciNot.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    HesaplaVeAtamaYap(ogrenciNot);
                    _context.Update(ogrenciNot);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OgrenciNotExists(ogrenciNot.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            await PopulateDropdownsAsync(ogrenciNot.OgrenciId, ogrenciNot.DersId);
            return View(ogrenciNot);
        }

        // POST: OgrenciNot/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var notKayit = await _context.OgrenciNotlar.FindAsync(id);
            if (notKayit != null)
            {
                _context.OgrenciNotlar.Remove(notKayit);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool OgrenciNotExists(int id)
        {
            return _context.OgrenciNotlar.Any(e => e.Id == id);
        }

        private async Task PopulateDropdownsAsync(int? selectedOgrenciId = null, int? selectedDersId = null)
        {
            ViewBag.OgrenciId = new SelectList(
                await _context.Ogrenciler
                    .Select(s => new { s.Id, AdSoyadNo = $"{s.OgrenciNo} - {s.Ad} {s.Soyad}" })
                    .ToListAsync(),
                "Id", "AdSoyadNo", selectedOgrenciId
            );

            ViewBag.DersId = new SelectList(
                await _context.Dersler
                    .Select(d => new { d.Id, DersKoduAdi = $"{d.DersKodu} - {d.DersAdi}" })
                    .ToListAsync(),
                "Id", "DersKoduAdi", selectedDersId
            );
        }

        private void HesaplaVeAtamaYap(OgrenciNot ogrenciNot)
        {
            double vize = ogrenciNot.VizeNotu.GetValueOrDefault(0);
            double sonSinav = ogrenciNot.ButunlemeNotu.HasValue
                ? ogrenciNot.ButunlemeNotu.Value
                : ogrenciNot.FinalNotu.GetValueOrDefault(0);

            double ortalama = (vize * 0.40) + (sonSinav * 0.60);
            ortalama = Math.Round(ortalama, 2);

            var sonuc = HesaplaHarfNotu(ortalama, sonSinav);
            ogrenciNot.HarfNotu = sonuc.HarfNotu;
            ogrenciNot.HarfNotuKatsayisi = sonuc.Katsayi;
            ogrenciNot.GectiMi = sonuc.GectiMi;
        }

        private (string HarfNotu, double Katsayi, bool GectiMi) HesaplaHarfNotu(double ortalama, double sonSinavNotu)
        {
            if (sonSinavNotu < 50) return ("FF", 0.0, false);

            if (ortalama >= 90) return ("AA", 4.0, true);
            if (ortalama >= 85) return ("BA", 3.5, true);
            if (ortalama >= 80) return ("BB", 3.0, true);
            if (ortalama >= 75) return ("CB", 2.5, true);
            if (ortalama >= 70) return ("CC", 2.0, true);
            if (ortalama >= 65) return ("DC", 1.5, true);
            if (ortalama >= 60) return ("DD", 1.0, true);
            if (ortalama >= 50) return ("FD", 0.5, false);
            return ("FF", 0.0, false);
        }
    }
}