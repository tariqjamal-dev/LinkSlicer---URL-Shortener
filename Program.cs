using LinkSlicer.Data;
using LinkSlicer.IServices;
using LinkSlicer.Models;
using LinkSlicer.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://0.0.0.0:5032"); // Equivalent to your old line

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedEmail = builder.Configuration.GetValue<bool>("EnableEmailConfirmation");
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Configure authentication
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

// Bind EmailSettings from appsettings.json
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

// Add email sender service (replace with real implementation)
builder.Services.AddTransient<IEmailSender, EmailSender>();  // ? Keep Transient (Stateless email sending)
builder.Services.AddScoped<IGeoLocationService, GeoLocationService>();  // ? Keep Scoped
builder.Services.AddScoped<IIpService, IpService>();  // ? Keep Scoped
builder.Services.AddScoped<ITrackingService, TrackingService>();  // ? Keep Scoped
builder.Services.AddScoped<IUrlShortenerService, UrlShortenerService>();  // ? Keep Scoped
builder.Services.AddScoped<IAccessLogService, AccessLogService>();  // ? Keep Scoped
builder.Services.AddSingleton<IBrowserInfoService, BrowserInfoService>();  // ?? Change to Singleton (Does not depend on requests)
builder.Services.AddScoped<IUrlService, UrlService>();  // ? Keep Scoped
builder.Services.AddScoped<IUserUrlService, UserUrlService>();



//builder.Services.AddControllersWithViews();
//builder.Services.AddRazorPages();

builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.Urls.Add("http://127.0.0.1:5032"); // Force IPv4
//webBuilder.UseUrls("http://0.0.0.0:5032");


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=UrlShortener}/{action=Index}/{id?}");

app.Run();
