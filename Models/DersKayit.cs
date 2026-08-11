using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OgrenciBilgiSistemi.Models
{
    public class DersKayit
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string OgrenciNo { get; set; } = string.Empty;

        // Ders tablosu ile ilişki kurabilmek için DersId ve navigation property ekliyoruz
        public int DersId { get; set; }

        [ForeignKey("DersId")]
        public Ders? Ders { get; set; }

        public string DersKodu { get; set; } = string.Empty;

        public int DonemYili { get; set; } = 20261;

        public bool DanismanOnaylandiMi { get; set; } = false;

        public string Durum { get; set; } = "Beklemede";

        public DateTime KayitTarihi { get; set; } = DateTime.Now;
    }
}