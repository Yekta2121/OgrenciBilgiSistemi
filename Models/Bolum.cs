using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OgrenciBilgiSistemi.Models
{
    [Table("bolumler")]
    public class Bolum
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Column("bolum_adi")]
        public string BolumAdi { get; set; } = string.Empty;

        [Column("mezuniyet_icin_gerekli_kredi")]
        public int MezuniyetIcinGerekliKredi { get; set; } = 120; // Lisans standart yerel kredi barajı

        public ICollection<Ogrenci>? Ogrenciler { get; set; }
    }
}