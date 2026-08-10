using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OgrenciBilgiSistemi.Data;
using OgrenciBilgiSistemi.Models;
using OgrenciBilgiSistemi.Servisler;

namespace OgrenciBilgiSistemi.Controllers
{
    [Authorize] // Sisteme giriş yapmış kullanıcılar erişebilir
    public class DersIslemleriController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly YoklamaVeDersMotoru _dersMotoru;

        public DersIslemleriController(ApplicationDbContext context, YoklamaVeDersMotoru dersMotoru)
        {
            _context = context;
            _dersMotoru = dersMotoru;
        }

        // 1. Öğrencinin Ders Seçim Ekranı (GET)
        [HttpGet]
        public IActionResult DersSecimi()
        {
            // Sistemdeki tüm dersleri listelemek üzere modele gönderiyoruz
            var dersler = _context.Dersler.ToList();
            return View(dersler);
        }

        // 2. Ders Seçim İşlemi (POST)
        [HttpPost]
        public IActionResult DersSecimiYap(string ogrenciNo, string dersKodu)
        {
            var sonuc = _dersMotoru.DersSecimiYap(ogrenciNo, dersKodu);

            TempData["Mesaj"] = sonuc.Mesaj;
            return RedirectToAction(nameof(DersSecimi));
        }

        // 3. Devamsızlık ve Not Durumunu Görüntüleme Ekranı
        public IActionResult Durumum(string ogrenciNo)
        {
            // Öğrencinin notlarını ve DZ durumlarını listeleyelim
            var notlar = _context.OgrenciNotlar
                .Where(n => n.Ogrenci != null && n.Ogrenci.OgrenciNo == ogrenciNo)
                .ToList();

            return View(notlar);
        }
    }
}