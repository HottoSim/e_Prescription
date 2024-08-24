using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using e_Prescription.Data;
using e_Prescription.Models.Account;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity.UI.Services;
using e_Prescription.EmailSender;
//using e_Prescription.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddTransient<e_Prescription.EmailSender.IEmailSender, EmailSender>();
// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure the database context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DBConnection")));

// Configure Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    });

// Register the IEmailSender service
//builder.Services.AddTransient<IEmailSender, EmailSender>();

// Add Razor Pages support
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Add authentication and authorization middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

// Map Razor Pages if used
app.MapRazorPages();

app.Run();
