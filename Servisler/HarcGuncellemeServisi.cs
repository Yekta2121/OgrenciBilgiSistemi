using Microsoft.EntityFrameworkCore;
using OgrenciBilgiSistemi.Data;
using OgrenciBilgiSistemi.Models;

namespace OgrenciBilgiSistemi.Servisler
{
    public class HarcGuncellemeServisi
    {
        private readonly ApplicationDbContext _context;

        public HarcGuncellemeServisi(ApplicationDbContext context)
        {
            _context = context;
        }

        // Bankadan gelen XML/API verilerini işleyip mutabakat sağlayan ideal metot
        public async Task<(bool Basarili, string Mesaj)> OdemeMutabakatiYapAsync(List<BankaOdemeBildirimi> bildirimler)
        {
            int guncellenenSayisi = 0;
            int hataliSayisi = 0;

            foreach (var bildirim in bildirimler)
            {
                var maliDurum = await _context.OgrenciMaliDurumlari
                    .FirstOrDefaultAsync(x => x.OgrenciNo == bildirim.OgrenciNo);

                if (maliDurum != null)
                {
                    // Öğrenci bulundu, borçtan ödenen tutarı düşüyoruz
                    maliDurum.HarcBorcu -= bildirim.OdenenTutar;
                    if (maliDurum.HarcBorcu < 0) maliDurum.HarcBorcu = 0; // Eksiye düşmesini engelle

                    maliDurum.SonOdemeTarihi = bildirim.IslemTarihi;
                    guncellenenSayisi++;
                }
                else
                {
                    // Öğrenci sistemde yoksa otomatik kayıt açmıyoruz, hatalı sayıyoruz
                    // İleride buraya "HatalıÖdemeler" log tablosu eklenebilir.
                    hataliSayisi++;
                }
            }

            await _context.SaveChangesAsync();
            return (true, $"{guncellenenSayisi} adet öğrencinin harç ödemesi başarıyla işlendi. {hataliSayisi} adet ödeme eşleşmeyen öğrenci numarası nedeniyle reddedildi.");
        }
    }
}