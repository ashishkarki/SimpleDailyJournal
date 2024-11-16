using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using SimpleDailyJournal.Data;
using Supabase;

var builder = WebApplication.CreateBuilder(args);

// Load environment variables from .env file
DotNetEnv.Env.Load(".env");

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register the DbContext to use SQLite
builder.Services.AddDbContext<JournalDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure Supabase client with URL and Key from environment variables
var supabaseUrl = Environment.GetEnvironmentVariable("SUPABASE_URL") ?? throw new InvalidOperationException("SUPABASE_URL is not set.");
var supabaseKey = Environment.GetEnvironmentVariable("SUPABASE_KEY") ?? throw new InvalidOperationException("SUPABASE_KEY is not set.");
var supabaseClient = new Client(supabaseUrl, supabaseKey, new SupabaseOptions
{
    AutoRefreshToken = true,
    // PersistSession = true
});

// Register the Supabase client as a singleton service
builder.Services.AddSingleton(supabaseClient);

// Add session services
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie();

// Build the app
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Add session and authentication middlewares
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

// Configure the routing configuration
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=JournalEntries}/{action=Index}/{id?}");

app.Run();