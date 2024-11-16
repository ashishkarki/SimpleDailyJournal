using Microsoft.EntityFrameworkCore;
using SimpleDailyJournal.Data;
using SimpleDailyJournal.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register the DbContext to use SQLite
builder.Services.AddDbContext<JournalDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSingleton<SentimentAnalysisService>();

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

app.UseAuthorization();

/*
 * Update the routing configuration so that any request to / (root) will automatically
 * be directed to the Index action of the JournalEntriesController
 */
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=JournalEntries}/{action=Index}/{id?}");

app.Run();