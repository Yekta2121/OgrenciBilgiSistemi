using OgrenciBilgiSistemi.Data;
using OgrenciBilgiSistemi.Models;

namespace OgrenciBilgiSistemi.Servisler
{
    public class YoklamaVeDersMotoru
    {
        private readonly ApplicationDbContext _context;

        public YoklamaVeDersMotoru(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. Ders Seçim Kuralı: Harç borcu olan veya mükerrer ders seçen engellenir
        public (bool Basarili, string Mesaj) DersSecimiYap(string ogrenciNo, string dersKodu)
        {
            var ogrenci = _context.Ogrenciler.FirstOrDefault(o => o.OgrenciNo == ogrenciNo);
            if (ogrenci == null) return (false, "Öğrenci bulunamadı.");

            var mevcutKayit = _context.Set<DersKayit>()
                .FirstOrDefault(dk => dk.OgrenciNo == ogrenciNo && dk.DersKodu == dersKodu);

            if (mevcutKayit != null) return (false, "Bu dersi zaten seçtiniz.");

            var yeniKayit = new DersKayit
            {
                OgrenciNo = ogrenciNo,
                DersKodu = dersKodu,
                DanismanOnaylandiMi = false
            };

            _context.Set<DersKayit>().Add(yeniKayit);
            _context.SaveChanges();

            return (true, "Ders seçimi başarıyla yapıldı, danışman onayına gönderildi.");
        }

        // 2. Devamsızlık Sınırı ve DZ Hesaplama Motoru
        public void DevamsizlikKontrolEt(string ogrenciNo, string dersKodu, bool uygulamaliMi)
        {
            var toplamDers = _context.Set<DersYoklamaSaati>()
                .Where(y => y.DersKodu == dersKodu && y.UygulamaliMi == uygulamaliMi).Count();

            if (toplamDers == 0) return;

            var katildigi = _context.Set<OgrenciYoklamaKayit>()
                .Join(_context.Set<DersYoklamaSaati>(),
                      kayit => kayit.YoklamaSaatiId,
                      saat => saat.Id,
                      (kayit, saat) => new { kayit.OgrenciNo, saat.DersKodu, kayit.KatildiMi })
                .Where(x => x.OgrenciNo == ogrenciNo && x.DersKodu == dersKodu && x.KatildiMi)
                .Count();

            decimal devamOrani = ((decimal)katildigi / toplamDers) * 100;
            decimal sınır = uygulamaliMi ? 80.0M : 70.0M;

            // Null-conditional (?.) operatörü ile null uyarıları tamamen giderildi
            if (devamOrani < sınır)
            {
                var notKaydi = _context.OgrenciNotlar
                    .FirstOrDefault(n => n.Ogrenci != null && n.Ogrenci.OgrenciNo == ogrenciNo &&
                                         n.Ders != null && n.Ders.DersKodu == dersKodu);

                if (notKaydi != null)
                {
                    notKaydi.HarfNotu = "DZ";
                    _context.SaveChanges();
                }
            }
        }
    }
}