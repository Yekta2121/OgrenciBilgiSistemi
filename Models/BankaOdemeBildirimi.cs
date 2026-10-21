namespace OgrenciBilgiSistemi.Models
{
    public class BankaOdemeBildirimi
    {
        public string OgrenciNo { get; set; } = string.Empty;
        public decimal OdenenTutar { get; set; }
        public DateTime IslemTarihi { get; set; }
        public string ReferansNo { get; set; } = string.Empty;
        public string BankaKodu { get; set; } = string.Empty; // VAKIF, ZIRAAT vb.
    }
}