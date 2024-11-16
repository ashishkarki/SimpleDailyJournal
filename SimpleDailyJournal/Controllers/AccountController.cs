using Microsoft.AspNetCore.Mvc;
using Supabase;
using System.Threading.Tasks;
using System;

namespace SimpleDailyJournal.Controllers;

public class AccountController : Controller
{
    private readonly Client _supabaseClient;

    public AccountController(Client supabaseClient)
    {
        _supabaseClient = supabaseClient;
    }

    public IActionResult Login()
    {
        // Load the Supabase URL from environment variables
        var supabaseUrl = Environment.GetEnvironmentVariable("SUPABASE_URL") ?? throw new ArgumentNullException("Environment.GetEnvironmentVariable(\"SUPABASE_URL\")");
        if (string.IsNullOrEmpty(supabaseUrl))
        {
            throw new InvalidOperationException("Supabase URL not configured properly in .env file.");
        }

        // Define the redirect URL after login
        var redirectUrl = Url.Action("AuthCallback", "Account", null, Request.Scheme);

        // Construct the login URL using the Supabase base URL
        var loginUrl = $"{supabaseUrl}/auth/v1/authorize?provider=email&redirect_to={Uri.EscapeDataString(redirectUrl)}";

        return Redirect(loginUrl);
    }

    public async Task<IActionResult> AuthCallback(string access_token, string refresh_token)
    {
        // Handle the callback from supabase and set the session
        if (string.IsNullOrEmpty(access_token))
        {
            return RedirectToAction("Login", "Account");
        }
        
        // Set session for the authenticated user
        await _supabaseClient.Auth.SetSession(access_token, refresh_token);
        
        return RedirectToAction("Index", "JournalEntries"); // redirect to Index action of JournalEntries Ctrl
    }
    
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        // Logout from Supabase
        await _supabaseClient.Auth.SignOut();
        return RedirectToAction("LoggedOutInfo", "Account");
    }

    public IActionResult LoggedOutInfo()
    {
        ViewBag.Message = "You have been logged out. Please log in to access your journal entries.";
        return View();
    }
}