using BulkyWeb.Data;
using BulkyWeb.Repository;
using BulkyWeb.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Bulky.Utility;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using Stripe;
using Bulky.DataAccess.DbInitializer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
//passing connecionstring to dbcontext class
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//configure stripe in project
builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));

builder.Services.AddIdentity<IdentityUser,IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
//for redirect to login, accessdenied path when user try with url without login
builder.Services.ConfigureApplicationCookie(options => {
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
});

//configuring facebook authentication
builder.Services.AddAuthentication().AddFacebook(option =>
{
    option.AppId = "1513396066057725";
    option.AppSecret = "4c86fb8c03973f2b1f53fcf83e7aad65";
});

//configuring microsoft acc
builder.Services.AddAuthentication().AddMicrosoftAccount(option =>
{
    option.ClientId = "f9efbe23-dbf5-47cb-9cc8-74c0b733fe7d";
    option.ClientSecret = "Lz78Q~a6-SRljjS3ZxCmeNrG6Of8KAKqrh8MabgD";
});

//adding session to services
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(100);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

//adding dbinitializer
builder.Services.AddScoped<IDbInitializer, DbInitializer>();
//aading razor page for login register for authentication
builder.Services.AddRazorPages();
// adding service to dependency injection
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
//registering IEmailSender implementation
builder.Services.AddScoped<IEmailSender, EmailSender>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:Secretkey").Get<string>();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
SeedDatabase();//invoking seeddatabase in pipeline
app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.Run();

//invoking Initialize() method from DbInitializer
void SeedDatabase()
{
    using (var scope = app.Services.CreateScope())
    {
        var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
        dbInitializer.Initialize();
    }
}