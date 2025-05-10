using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using PharmacyInventorySystem.Data;
using PharmacyInventorySystem.Models;
using PharmacyInventorySystem.Services;
using Rotativa.AspNetCore;
using DinkToPdf;
using DinkToPdf.Contracts;

var builder = WebApplication.CreateBuilder(args);

// -----------------------
// خدمات عامة
// -----------------------
builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
builder.Services.AddScoped<PdfGenerator>();

// -----------------------
// الاتصال بقاعدة البيانات
// -----------------------
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// -----------------------
// الهوية والمصادقة
// -----------------------
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// -----------------------
// إعداد الإيميل
// -----------------------
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddTransient<IEmailSender, EmailSender>();

// -----------------------
// إعداد الكوكيز
// -----------------------
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
});
// -----------------------
// Manager Supplier Order
// -----------------------
builder.Services.AddScoped<ISupplierOrderService, SupplierOrderService>();

// -----------------------
// MVC & Razor
// -----------------------
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// -----------------------
// إعداد Rotativa
// -----------------------
RotativaConfiguration.Setup(app.Environment.WebRootPath, "Rotativa");

// -----------------------
// Middleware
// -----------------------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}




app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// -----------------------
// إعادة توجيه /login
// -----------------------
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/login")
    {
        context.Response.Redirect("/Identity/Account/Login");
        return;
    }

    await next();
});

// -----------------------
// توجيه المستخدم حسب الدور عند الوصول إلى الصفحة الرئيسية
// -----------------------
app.Use(async (context, next) =>
{
    var user = context.User;
    var signInManager = context.RequestServices.GetRequiredService<SignInManager<ApplicationUser>>();
    var userManager = context.RequestServices.GetRequiredService<UserManager<ApplicationUser>>();

    if (!user.Identity.IsAuthenticated && context.Request.Path == "/")
    {
        context.Response.Redirect("/Identity/Account/Login");
        return;
    }

    if (user.Identity.IsAuthenticated && context.Request.Path == "/")
    {
        var currentUser = await userManager.GetUserAsync(user);
        var roles = await userManager.GetRolesAsync(currentUser);

        if (roles.Contains("Manager"))
        {
            context.Response.Redirect("/Manager/Home/Dashboard");
            return;
        }
        else if (roles.Contains("Pharmacist"))
        {
            context.Response.Redirect("/Pharmacist/Home/Dashboard");
            return;
        }
    }

    await next();
});

// -----------------------
// مسارات Controllers + Razor
// -----------------------
app.MapControllerRoute(
    name: "default",
    pattern: "{area=Pharmacist}/{controller=Home}/{action=Dashboard}/{id?}");

app.MapRazorPages();

// -----------------------
// تهيئة البيانات الابتدائية
// -----------------------
await SeedData.InitializeAsync(app);

app.Run();
