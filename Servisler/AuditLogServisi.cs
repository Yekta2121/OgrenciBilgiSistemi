using OgrenciBilgiSistemi.Data;
using OgrenciBilgiSistemi.Models;

namespace OgrenciBilgiSistemi.Servisler
{
    public class AuditLogServisi
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuditLogServisi(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public void LogEkle(string islem, string detay)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var kullaniciEmail = httpContext?.User?.Identity?.Name ?? "Misafir / Anonim";
            var ipAdresi = httpContext?.Connection?.RemoteIpAddress?.ToString() ?? "Bilinmiyor";

            var log = new AuditLog
            {
                KullaniciEmail = kullaniciEmail,
                Islem = islem,
                Detay = detay,
                IpAdresi = ipAdresi,
                ZamanDamgasi = DateTime.UtcNow // PostgreSQL UTC uyumluluğu için düzeltildi
            };

            _context.AuditLogs.Add(log);
            _context.SaveChanges();
        }
    }
}