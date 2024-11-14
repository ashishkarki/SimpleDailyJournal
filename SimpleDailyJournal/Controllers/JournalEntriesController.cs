using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleDailyJournal.Data;
using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Authorization;
using SimpleDailyJournal.Models;

namespace SimpleDailyJournal.Controllers
{
    [Authorize]
    public class JournalEntriesController : Controller
    {
        private readonly JournalDbContext _context;

        public JournalEntriesController(JournalDbContext context)
        {
            _context = context;
        }

        ////  CRUD methods below
        // GET: JournalEntries
        public async Task<IActionResult> Index()
        {
            var userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
            var userEntries = _context.JournalEntries
                .Where(entry => entry.UserId == userId)
                .ToListAsync();

            return View(await userEntries);
        }

        // GET: JournalEntries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            return await GetJournalEntryOrNotFound(id);
        }

        // GET: JournalEntries/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: JournalEntries/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Date, Content, Mood")] JournalEntry journalEntry)
        {
            if (!ModelState.IsValid) return View(journalEntry);

            journalEntry.UserId = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value!;
            _context.Add(journalEntry);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: JournalEntries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            return await GetJournalEntryOrNotFound(id);
        }

        // POST: JournalEntries/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id, Date, Content, Mood")] JournalEntry journalEntry)
        {
            Guard.Against.Null(id, nameof(id));

            if (id != journalEntry.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid) return View(journalEntry);

            try
            {
                _context.Update(journalEntry);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JournalEntryExists(journalEntry.Id))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: JournalEntries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            return await GetJournalEntryOrNotFound(id);
        }

        // POST: JournalEntries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var journalEntry = await _context.JournalEntries.FindAsync(id);

            if (journalEntry == null) return RedirectToAction(nameof(Index));

            _context.JournalEntries.Remove(journalEntry);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        //// Helper methods below this
        // Helper method to reduce redundancy
        private async Task<IActionResult> GetJournalEntryOrNotFound(int? id)
        {
            Guard.Against.Null(id, nameof(id));

            var journalEntry = await _context.JournalEntries.FindAsync(id);
            return journalEntry == null ? NotFound() : View(journalEntry);
        }

        private bool JournalEntryExists(int id)
        {
            return _context.JournalEntries.Any(e => e.Id == id);
        }
    }
}