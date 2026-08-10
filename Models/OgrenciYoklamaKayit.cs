using System.ComponentModel.DataAnnotations;

namespace OgrenciBilgiSistemi.Models
{
    public class OgrenciYoklamaKayit
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int YoklamaSaatiId { get; set; }

        [Required]
        public string OgrenciNo { get; set; } = string.Empty;

        public bool KatildiMi { get; set; } = false;
    }
}