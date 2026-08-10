using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OgrenciBilgiSistemi.Data;
using OgrenciBilgiSistemi.Servisler; // Servis sınıfının namespace'i eklendi

var builder = WebApplication.CreateBuilder(args);

// PostgreSQL Database Connection
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// ASP.NET Core Identity ve Rol Yönetimi Eklendi
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>();

// --- YENİ EKLENEN KURUMSAL SERVİSLER ---
builder.Services.AddScoped<YoklamaVeDersMotoru>();
builder.Services.AddHttpContextAccessor(); // AuditLog için HTTP bağlamı erişimi
builder.Services.AddScoped<AuditLogServisi>(); // KVKK ve 5651 Loglama Servisi
builder.Services.AddScoped<BelgeVeTalepServisi>(); // Belge ve Talep Yönetimi Servisi
builder.Services.AddScoped<PdfBelgeUreticiServisi>(); // E-İmzalı PDF Üretim Servisi
// ----------------------------------------

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

// DİKKAT: UseAuthentication, UseAuthorization'dan ÖNCE yazılmalıdır!
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

// Scaffold edilen Identity Razor Pages sayfalarının çalışabilmesi için zorunludur:
app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

// --- Otomatik Rol ve Admin Kullanıcısı Oluşturma (Seeding) ---
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

        string[] roller = { "Admin", "Personel", "Ogrenci", "Akademisyen", "BolumBaskani", "OgrenciIsleri", "SistemYoneticisi" };

        foreach (var rol in roller)
        {
            if (!await roleManager.RoleExistsAsync(rol))
            {
                await roleManager.CreateAsync(new IdentityRole(rol));
            }
        }

        var adminEmail = "admin@obs.edu.tr";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            var yeniAdmin = new IdentityUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };

            var sonuc = await userManager.CreateAsync(yeniAdmin, "Admin123*");
            if (sonuc.Succeeded)
            {
                await userManager.AddToRoleAsync(yeniAdmin, "Admin");
            }
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Roller ve admin oluşturulurken bir hata meydana geldi.");
    }
}
// -------------------------------------------------------------

app.Run();