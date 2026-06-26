using Microsoft.Extensions.FileProviders;
using Portfolio_Management_System.DAL;
using Portfolio_Management_System.Hubs;
using Portfolio_Management_System.Models;
using Portfolio_Management_System.Services;


var builder = WebApplication.CreateBuilder(args);
// Add services to container section ??? ?? line add ???
builder.Services.AddHttpClient();
// Add services
builder.Services.AddControllersWithViews();

// Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.Name = ".Portfolio.Session";
});

// HttpContext
builder.Services.AddHttpContextAccessor();

// Database
builder.Services.AddScoped<DbHelper>();

// Email
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<IEmailService, EmailService>();

// SignalR
builder.Services.AddSignalR();

var app = builder.Build();

// Configure pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

// Create upload directories
var uploadsPath = Path.Combine(app.Environment.WebRootPath, "uploads");
Directory.CreateDirectory(Path.Combine(uploadsPath, "photos"));
Directory.CreateDirectory(Path.Combine(uploadsPath, "resumes"));
Directory.CreateDirectory(Path.Combine(uploadsPath, "projects"));

// Routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<ChatHub>("/chathub");

app.Run();