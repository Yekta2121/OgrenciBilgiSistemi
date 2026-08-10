using OgrenciBilgiSistemi.Data;
using OgrenciBilgiSistemi.Models;

namespace OgrenciBilgiSistemi.Servisler
{
    public class BelgeVeTalepServisi
    {
        private readonly ApplicationDbContext _context;
        private readonly AuditLogServisi _auditLogServisi;

        public BelgeVeTalepServisi(ApplicationDbContext context, AuditLogServisi auditLogServisi)
        {
            _context = context;
            _auditLogServisi = auditLogServisi;
        }

        // Barkodlu Belge Talebi Oluşturma
        public BelgeTalebi BelgeTalepEt(string ogrenciNo, string belgeTuru)
        {
            var barkod = "OBS-" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper();

            var talep = new BelgeTalebi
            {
                OgrenciNo = ogrenciNo,
                BelgeTuru = belgeTuru,
                BarkodKodu = barkod,
                TalepTarihi = DateTime.UtcNow, // UTC formatına çevrildi
                Durum = "Onaylandı ve Üretildi"
            };

            _context.BelgeTalepleri.Add(talep);
            _context.SaveChanges();

            // 5651 / KVKK Audit Log Kaydı
            _auditLogServisi.LogEkle("Belge Talebi", $"{ogrenciNo} nolu öğrenci {belgeTuru} talep etti. Barkod: {barkod}");

            return talep;
        }

        // Dilekçe / Mazeret Başvurusu Oluşturma
        public void DilekceGonder(string ogrenciNo, string basvuruTuru, string aciklama)
        {
            var dilekce = new DilekceBasvurusu
            {
                OgrenciNo = ogrenciNo,
                BasvuruTuru = basvuruTuru,
                Aciklama = aciklama,
                BasvuruTarihi = DateTime.UtcNow, // UTC formatına çevrildi
                OnayDurumu = "Beklemede"
            };

            _context.DilekceBasvurulari.Add(dilekce);
            _context.SaveChanges();

            _auditLogServisi.LogEkle("Dilekçe Başvurusu", $"{ogrenciNo} nolu öğrenci {basvuruTuru} türünde başvuru oluşturdu.");
        }
    }
}