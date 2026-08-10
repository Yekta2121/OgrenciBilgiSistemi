namespace OgrenciBilgiSistemi.Models
{
    public class DilekceBasvurusu
    {
        public int Id { get; set; }
        public string OgrenciNo { get; set; } = string.Empty;
        public string BasvuruTuru { get; set; } = string.Empty; // Sınav Mazereti, Kayıt Dondurma, Yatay Geçiş
        public string Aciklama { get; set; } = string.Empty;
        public DateTime BasvuruTarihi { get; set; } = DateTime.Now;
        public string OnayDurumu { get; set; } = "Beklemede"; // Beklemede, Bölüm Onayladı, Reddedildi
    }
}