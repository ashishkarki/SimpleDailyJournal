using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SimpleDailyJournal.Data;

var builder = WebApplication.CreateBuilder(args);

// Register the DbContext to use SQLite
builder.Services.AddDbContext<JournalDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Identity services
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<JournalDbContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();

// the app
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

app.UseAuthentication(); // enable authn: who can login
app.UseAuthorization(); // enable authorization: who can access what

/**
 * Update the routing configuration so that any request to / (root) will automatically
 * be directed to the Index action of the JournalEntriesController
 */
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=JournalEntries}/{action=Index}/{id?}");

app.Run();