using Auth0.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using SimpleDailyJournal.Data;

var builder = WebApplication.CreateBuilder(args);

// load environment variables from .env file
DotNetEnv.Env.Load(".env");

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register the DbContext to use SQLite
builder.Services.AddDbContext<JournalDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// configure Auth0 authentication
builder.Services.AddAuth0WebAppAuthentication(options =>
{
    options.Domain = Environment.GetEnvironmentVariable("AUTH0_DOMAIN");
    options.ClientId = Environment.GetEnvironmentVariable("AUTH0_CLIENT_ID");
    options.ClientSecret = Environment.GetEnvironmentVariable("AUTH0_CLIENT_SECRET");
    options.Scope = "openid profile email";
    options.CallbackPath = new PathString("/Account/AuthCallback");
});

// the app for any configuration
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

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

/**
 * Update the routing configuration so that any request to / (root) will automatically
 * be directed to the Index action of the JournalEntriesController
 */
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=JournalEntries}/{action=Index}/{id?}");

app.Run();