using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OgrenciBilgiSistemi.Data;
using OgrenciBilgiSistemi.Servisler;

namespace OgrenciBilgiSistemi.Controllers
{
    [Authorize] // Sisteme giriş yapmış kullanıcılar erişebilir
    public class BelgeTalepController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly BelgeVeTalepServisi _belgeServisi;
        private readonly PdfBelgeUreticiServisi _pdfServisi;

        public BelgeTalepController(ApplicationDbContext context, BelgeVeTalepServisi belgeServisi, PdfBelgeUreticiServisi pdfServisi)
        {
            _context = context;
            _belgeServisi = belgeServisi;
            _pdfServisi = pdfServisi;
        }

        // 1. Belge Talep Etme Sayfası (GET)
        [HttpGet]
        public IActionResult Index()
        {
            var talepler = _context.BelgeTalepleri.ToList();
            return View(talepler);
        }

        // 2. Barkodlu Belge Üretme İşlemi (POST)
        [HttpPost]
        public IActionResult TalepOlustur(string ogrenciNo, string belgeTuru)
        {
            _belgeServisi.BelgeTalepEt(ogrenciNo, belgeTuru);
            TempData["Mesaj"] = $"{belgeTuru} başarıyla barkodlu olarak üretildi.";
            return RedirectToAction(nameof(Index));
        }

        // 3. E-İmzalı PDF Belgeyi İndirme İşlemi (GET)
        [HttpGet]
        public IActionResult PdfIndir(int id)
        {
            var talep = _context.BelgeTalepleri.Find(id);
            if (talep == null) return NotFound();

            var pdfBytes = _pdfServisi.BelgePdfUret(talep.OgrenciNo, talep.BelgeTuru, talep.BarkodKodu);
            return File(pdfBytes, "application/pdf", $"{talep.BelgeTuru}_{talep.BarkodKodu}.pdf");
        }

        // 4. Dilekçe ve Mazeret Başvuru Ekranı (GET)
        [HttpGet]
        public IActionResult DilekceBasvuru()
        {
            var dilekceler = _context.DilekceBasvurulari.ToList();
            return View(dilekceler);
        }

        // 5. Dilekçe Gönderme İşlemi (POST)
        [HttpPost]
        public IActionResult DilekceGonder(string ogrenciNo, string basvuruTuru, string aciklama)
        {
            _belgeServisi.DilekceGonder(ogrenciNo, basvuruTuru, aciklama);
            TempData["Mesaj"] = "Dilekçe ve mazeret başvurunuz ilgili birime iletildi.";
            return RedirectToAction(nameof(DilekceBasvuru));
        }
    }
}