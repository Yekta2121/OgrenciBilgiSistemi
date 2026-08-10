namespace OgrenciBilgiSistemi.Models
{
    public class AuditLog
    {
        public int Id { get; set; }
        public string KullaniciEmail { get; set; } = string.Empty;
        public string Islem { get; set; } = string.Empty; // Örn: Not Güncelleme, Belge Erişimi
        public string Detay { get; set; } = string.Empty;
        public string IpAdresi { get; set; } = string.Empty;
        public DateTime ZamanDamgasi { get; set; } = DateTime.UtcNow;
    }
}