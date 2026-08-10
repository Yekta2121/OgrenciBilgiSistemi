namespace OgrenciBilgiSistemi.Models
{
    public class BelgeTalebi
    {
        public int Id { get; set; }
        public string OgrenciNo { get; set; } = string.Empty;
        public string BelgeTuru { get; set; } = string.Empty; // Örn: Öğrenci Belgesi, Transkript
        public string BarkodKodu { get; set; } = string.Empty; // Karekod/Barkod Doğrulama Kodu
        public DateTime TalepTarihi { get; set; } = DateTime.Now;
        public string Durum { get; set; } = "Hazırlanıyor"; // Hazırlanıyor, Onaylandı
    }
}