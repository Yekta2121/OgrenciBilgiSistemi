using System.ComponentModel.DataAnnotations;

namespace OgrenciBilgiSistemi.Models
{
    public class DersYoklamaSaati
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string DersKodu { get; set; } = string.Empty;

        public int HaftaNo { get; set; }

        public DateTime Tarih { get; set; } = DateTime.Now;

        public bool UygulamaliMi { get; set; } = false;
    }
}