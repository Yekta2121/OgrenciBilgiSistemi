using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OgrenciBilgiSistemi.Models
{
    [Table("ogrenci_notlari")]
    public class OgrenciNot
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("ogrenci_id")]
        public int OgrenciId { get; set; }

        [ForeignKey(nameof(OgrenciId))]
        public Ogrenci? Ogrenci { get; set; }

        [Column("ders_id")]
        public int DersId { get; set; }

        [ForeignKey(nameof(DersId))]
        public Ders? Ders { get; set; }

        [Range(0, 100)]
        [Column("vize_notu")]
        public double? VizeNotu { get; set; }

        [Range(0, 100)]
        [Column("final_notu")]
        public double? FinalNotu { get; set; }

        [Range(0, 100)]
        [Column("butunleme_notu")]
        public double? ButunlemeNotu { get; set; }

        [Column("harf_notu_katsayisi")]
        public double HarfNotuKatsayisi { get; set; }

        [StringLength(2)]
        [Column("harf_notu")]
        public string HarfNotu { get; set; } = string.Empty;

        [Column("gecti_mi")]
        public bool GectiMi { get; set; }
    }
}