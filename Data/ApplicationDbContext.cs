using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OgrenciBilgiSistemi.Models;

namespace OgrenciBilgiSistemi.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Ogrenci> Ogrenciler { get; set; } = null!;
        public DbSet<Ders> Dersler { get; set; } = null!;
        public DbSet<OgrenciNot> OgrenciNotlar { get; set; } = null!;
        public DbSet<OgrenciNot> OgrenciNotlari => OgrenciNotlar; // Alias (Uyum için)
        public DbSet<OgrenciMaliDurum> OgrenciMaliDurumlari { get; set; } = null!;
        public DbSet<Bolum> Bolumler { get; set; } = null!;

        // Kurumsal Modüller
        public DbSet<DersYoklamaSaati> DersYoklamaSaatleri { get; set; } = null!;
        public DbSet<OgrenciYoklamaKayit> OgrenciYoklamaKayitlari { get; set; } = null!;
        public DbSet<DersKayit> DersKayitlari { get; set; } = null!;

        // KVKK ve 5651 Uyumlu Denetim Logları Tablosu
        public DbSet<AuditLog> AuditLogs { get; set; } = null!;

        // Belge ve Talep Yönetimi Tabloları
        public DbSet<BelgeTalebi> BelgeTalepleri { get; set; } = null!;
        public DbSet<DilekceBasvurusu> DilekceBasvurulari { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Identity tablolarının kendi konfigürasyonları için bu şarttır
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Ogrenci>()
                .HasIndex(o => o.TcKimlikNo)
                .IsUnique()
                .HasDatabaseName("ux_ogrenciler_tc_kimlik_no");

            modelBuilder.Entity<Ogrenci>()
                .HasIndex(o => o.OgrenciNo)
                .IsUnique()
                .HasDatabaseName("ux_ogrenciler_ogrenci_no");
        }
    }
}