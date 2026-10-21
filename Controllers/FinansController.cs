using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OgrenciBilgiSistemi.Models;
using OgrenciBilgiSistemi.Servisler;

namespace OgrenciBilgiSistemi.Controllers
{
    [Authorize(Roles = "Admin,OgrenciIsleri")]
    public class FinansController : Controller
    {
        private readonly HarcGuncellemeServisi _harcGuncellemeServisi;

        public FinansController(HarcGuncellemeServisi harcGuncellemeServisi)
        {
            _harcGuncellemeServisi = harcGuncellemeServisi;
        }

        // Simülasyon Test Ekranı (GET)
        [HttpGet]
        public IActionResult OdemeSimulasyonu()
        {
            return View();
        }

        // Banka Webhook / Mutabakat Simülasyon Tetikleyicisi (POST)
        [HttpPost]
        public async Task<IActionResult> OdemeSimulasyonuYap(string ogrenciNo, decimal odenenTutar, string bankaKodu)
        {
            var bildirimListesi = new List<BankaOdemeBildirimi>
            {
                new BankaOdemeBildirimi
                {
                    OgrenciNo = ogrenciNo,
                    OdenenTutar = odenenTutar,
                    IslemTarihi = DateTime.Now,
                    ReferansNo = "REF-" + new Random().Next(100000, 999999),
                    BankaKodu = bankaKodu
                }
            };

            var sonuc = await _harcGuncellemeServisi.OdemeMutabakatiYapAsync(bildirimListesi);

            TempData["Mesaj"] = sonuc.Mesaj;
            return RedirectToAction("OdemeSimulasyonu");
        }
    }
}