using System.ComponentModel.DataAnnotations;

namespace OgrenciBilgiSistemi.Models
{
    public class DersKayit
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string OgrenciNo { get; set; } = string.Empty;

        [Required]
        public string DersKodu { get; set; } = string.Empty;

        public int DonemYili { get; set; } = 20261;

        public bool DanismanOnaylandiMi { get; set; } = false;

        public DateTime KayitTarihi { get; set; } = DateTime.Now;
    }
}