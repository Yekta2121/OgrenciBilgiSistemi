using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OgrenciBilgiSistemi.Models
{
    [Table("ogrenciler")]
    public class Ogrenci
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [StringLength(11)]
        [Column("tc_kimlik_no")]
        public string TcKimlikNo { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        [Column("ogrenci_no")]
        public string OgrenciNo { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        [Column("ad")]
        public string Ad { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        [Column("soyad")]
        public string Soyad { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [Column("kurumsal_eposta")]
        public string KurumsalEposta { get; set; } = string.Empty;

        [Column("sinif")]
        public int Sinif { get; set; } = 1;

        [Column("kayit_turu")]
        public string KayitTuru { get; set; } = "YKS";

        [Column("durum_aktif_mi")]
        public bool DurumAktifMi { get; set; } = true;

        [Column("olusturma_tarihi")]
        public DateTime OlusturmaTarihi { get; set; } = DateTime.UtcNow;

        // Bölüm İlişkisi
        [Column("bolum_id")]
        public int BolumId { get; set; }

        [ForeignKey("BolumId")]
        public Bolum? Bolum { get; set; }

        public ICollection<OgrenciNot>? OgrenciNotlari { get; set; }
    }
}