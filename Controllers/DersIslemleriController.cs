using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OgrenciBilgiSistemi.Data;
using OgrenciBilgiSistemi.Servisler;

namespace OgrenciBilgiSistemi.Controllers
{
    [Authorize]
    public class DersIslemleriController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly AkademikDersKayitMotoru _dersKayitMotoru;

        public DersIslemleriController(ApplicationDbContext context, AkademikDersKayitMotoru dersKayitMotoru)
        {
            _context = context;
            _dersKayitMotoru = dersKayitMotoru;
        }

        [HttpGet]
        public async Task<IActionResult> DersSecimi(string ogrenciNo = "2026001")
        {
            ViewBag.OgrenciNo = ogrenciNo;
            var dersler = await _context.Dersler.ToListAsync();
            return View(dersler);
        }

        [HttpPost]
        public async Task<IActionResult> DersSecimiYap(string ogrenciNo, int dersId)
        {
            var sonuc = await _dersKayitMotoru.DersSecimiYapAsync(ogrenciNo, dersId);

            if (sonuc.Basarili)
                TempData["Mesaj"] = sonuc.Mesaj;
            else
                TempData["Hata"] = sonuc.Mesaj;

            return RedirectToAction(nameof(DersSecimi), new { ogrenciNo });
        }

        public async Task<IActionResult> Durumum(string ogrenciNo)
        {
            var notlar = await _context.OgrenciNotlar
                .Include(n => n.Ogrenci)
                .Include(n => n.Ders)
                .Where(n => n.Ogrenci != null && n.Ogrenci.OgrenciNo == ogrenciNo)
                .ToListAsync();

            return View(notlar);
        }
    }
}