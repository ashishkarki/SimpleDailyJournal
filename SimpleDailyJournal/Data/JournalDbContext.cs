using Microsoft.EntityFrameworkCore;
using SimpleDailyJournal.Models;

namespace SimpleDailyJournal.Data;

public class JournalDbContext : DbContext
{
    public JournalDbContext(DbContextOptions<JournalDbContext> options) : base(options)
    {
    }

    // Define a DbSet for each entity you want to store in the database
    public DbSet<JournalEntry> JournalEntries { get; set; }
}