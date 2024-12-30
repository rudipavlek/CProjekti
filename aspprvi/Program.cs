using aspprvi.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Postavi vremensko ograničenje za sesiju
    options.Cookie.HttpOnly = true;  // Postavi cookie da bude samo za HTTP (ne za JavaScript)
    options.Cookie.IsEssential = true; 
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Osigurava da se cookie šalje samo putem HTTPS-a
    options.Cookie.SameSite = SameSiteMode.Strict; 
    options.Cookie.MaxAge = TimeSpan.FromMinutes(30); 


});

// Dodavanje autentifikacije s Cookie Authentication shemom
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        // Putanja za prijavu (ako korisnik nije prijavljen, bit će preusmjeren na ovu stranicu)
        options.LoginPath = "/Login/Login";
        options.LogoutPath = "/Login/Logout";
        options.AccessDeniedPath = "/Login/AccessDenied";

    });

builder.Services.AddAuthorization();


// Add services to the container.
builder.Services.AddControllersWithViews();



builder.Services.AddDbContext<UserDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("blogKonekcija")));

builder.Services.AddDbContext<BlogPostDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("blogKonekcija")));

builder.Services.AddDbContext<CommentDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("blogKonekcija")));

var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseRouting();

app.UseSession();

app.UseHttpsRedirection();
app.UseStaticFiles();



app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Login}");


app.MapControllerRoute(
    name: "Login",
    pattern: "{controller=Login}/{action=Login}");

app.MapControllerRoute(
    name: "Admin",
    pattern: "{controller=Admin}/{action=Index}");

app.MapControllerRoute(
    name: "Blog",
    pattern: "{controller=Blog}/{action=Index}");

app.Run();
