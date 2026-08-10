using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OgrenciBilgiSistemi.Models
{
    [Table("dersler")]
    public class Ders
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        [Column("ders_kodu")]
        public string DersKodu { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        [Column("ders_adi")]
        public string DersAdi { get; set; } = string.Empty;

        [Column("teori_saat")]
        public int TeoriSaat { get; set; }

        [Column("uygulama_saat")]
        public int UygulamaSaat { get; set; }

        [Column("yerel_kredi")]
        public int YerelKredi { get; set; }

        [Column("akts")]
        public int Akts { get; set; }

        [Column("yariyil")]
        public int Yariyil { get; set; }

        [Column("zorunlu_mu")]
        public bool ZorunluMu { get; set; } = true;

        // AGNO hesabı için eklenen Kredi alanı (Varsayılan 5)
        [Range(1, 30)]
        [Column("kredi")]
        public int Kredi { get; set; } = 5;

        public virtual ICollection<OgrenciNot>? OgrenciNotlari { get; set; }
    }
}