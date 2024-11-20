using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleDailyJournal.Data;
using Ardalis.GuardClauses;
using SimpleDailyJournal.Models;
using SimpleDailyJournal.Services;
using SimpleDailyJournal.ViewModels;

namespace SimpleDailyJournal.Controllers
{
    public class JournalEntriesController : Controller
    {
        private readonly JournalDbContext _context;
        private readonly SentimentAnalysisService _sentimentAnalysisService;

        public JournalEntriesController(JournalDbContext context, SentimentAnalysisService sentimentAnalysisService)
        {
            _context = context;
            _sentimentAnalysisService = sentimentAnalysisService;
        }

        ////  CRUD methods below
        // GET: JournalEntries
        public async Task<IActionResult> Index()
        {
            return View(await _context.JournalEntries.ToListAsync());
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
        
        // Analyze sentiments action
        public IActionResult Analyze()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PerformAnalysis(DateRangeViewModel dateRange)
        {
            Guard.Against.Null(dateRange, nameof(dateRange));
            
            // fetch journal entries within the date range
            var journalEntriesInRange = await _context.JournalEntries
                .Where(entry => entry.Date >= dateRange.StartDate && entry.Date <= dateRange.EndDate)
                .ToListAsync();
            
            var validEntries = journalEntriesInRange.Where(entry => !string.IsNullOrWhiteSpace(entry.Content) && entry.Content.Length > 3).ToList();
            
            // If there are no entries within the specified range,
            // and those entries that are present are invalid (empty strings or very short strings),
            // the app should gracefully handle this and show an appropriate message
            if (validEntries.Count == 0)
            {
                ViewBag.Message = "No valid entries with sufficient content to analyze.";
                return View("AnalysisResult", new List<JournalEntry>());
            }
            
            // if (journalEntriesInRange.Count == 0)
            // {
            //     ViewBag.Message = "No journal entries found for the selected date range!!";
            //     return View("AnalysisResult", new List<JournalEntry>());
            // }
            
            // Log or store skipped entries for debugging purposes
            var skippedEntries = journalEntriesInRange.Count - validEntries.Count;

            if (skippedEntries > 0)
            {
                ViewBag.Warning = $"{skippedEntries} entries were too short or empty and were skipped.";
            }
            
            // perform sentiment analysis on each entry
            var analyzedEntries = validEntries.Select(entry => new
            {
                entry.Date,
                entry.Content,
                entry.Mood,
                Sentiment = _sentimentAnalysisService.PredictSentiment(entry.Content)
            }).ToList();
            
            // pass analyzed data to a named view called "AnalysisResult(.cshtml)"
            return View("AnalysisResult", analyzedEntries);
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