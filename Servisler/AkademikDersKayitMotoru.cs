using Microsoft.EntityFrameworkCore;
using OgrenciBilgiSistemi.Data;
using OgrenciBilgiSistemi.Models;

namespace OgrenciBilgiSistemi.Servisler
{
    public class AkademikDersKayitMotoru
    {
        private readonly ApplicationDbContext _context;

        public AkademikDersKayitMotoru(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<(bool Basarili, string Mesaj)> DersSecimiYapAsync(string ogrenciNo, int dersId)
        {
            // 1. Harç / Borç Kontrolü
            var maliDurum = await _context.OgrenciMaliDurumlari
                .FirstOrDefaultAsync(x => x.OgrenciNo == ogrenciNo);

            if (maliDurum != null && maliDurum.HarcBorcu > 0)
            {
                return (false, "Harç borcunuz bulunduğundan ders seçimi ekranınız bloke edilmiştir. Lütfen ödeme yapınız.");
            }

            // 2. Alınmak İstenen Dersin Bilgileri
            var hedefDers = await _context.Dersler.FindAsync(dersId);
            if (hedefDers == null) return (false, "Seçilen ders bulunamadı.");

            // 3. AKTS Sınır Kontrolü (Dönem toplamı max 30 AKTS varsayımı)
            var mevcutSecilenler = await _context.DersKayitlari
                .Include(d => d.Ders)
                .Where(x => x.OgrenciNo == ogrenciNo && x.Durum != "Reddedildi")
                .ToListAsync();

            int toplamAkts = mevcutSecilenler.Sum(x => x.Ders.Akts) + hedefDers.Akts;
            if (toplamAkts > 30)
            {
                return (false, $"AKTS sınırını aştınız! Mevcut toplam: {toplamAkts} AKTS (İzin verilen max: 30 AKTS).");
            }

            // 4. Ön Koşul (Prerequisite) Kontrolü (İlişkisel Ogrenci.OgrenciNo üzerinden)
            if (hedefDers.OnKosulDersId.HasValue)
            {
                bool onKosulVerildiMi = await _context.OgrenciNotlar
                    .Include(n => n.Ogrenci)
                    .AnyAsync(x => x.Ogrenci != null && x.Ogrenci.OgrenciNo == ogrenciNo && x.DersId == hedefDers.OnKosulDersId.Value && x.HarfNotu != "FF" && x.HarfNotu != "DZ");

                if (!onKosulVerildiMi)
                {
                    return (false, "Bu dersi alabilmeniz için ön koşul olan alt dönem dersini başarmış olmalısınız.");
                }
            }

            // 5. Kaydı Oluştur
            var yeniKayit = new DersKayit
            {
                OgrenciNo = ogrenciNo,
                DersId = dersId,
                DersKodu = hedefDers.DersKodu,
                DanismanOnaylandiMi = false,
                Durum = "Danışman Onayı Bekliyor",
                KayitTarihi = DateTime.UtcNow // PostgreSQL zaman dilimi uyumluluğu için UtcNow yapıldı
            };

            _context.DersKayitlari.Add(yeniKayit);
            await _context.SaveChangesAsync();

            return (true, "Ders başarıyla seçildi ve danışman onayına gönderildi.");
        }
    }
}