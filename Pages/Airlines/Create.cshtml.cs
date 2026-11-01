using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using wt_lab5_5razor_rabtsevich.Data;
using wt_lab5_5razor_rabtsevich.Models;

namespace wt_lab5_5razor_rabtsevich.Pages.Airlines;

public class CreateModel : PageModel
{
    [BindProperty]
    public Airline Airline { get; set; } = new();

    public void OnGet() { }

    public IActionResult OnPost()
    {
        // Нормализация
        Airline.Name = Airline.Name?.Trim() ?? string.Empty;
        Airline.IataCode = (Airline.IataCode ?? "").Trim().ToUpperInvariant();
        Airline.Country = Airline.Country?.Trim() ?? string.Empty;
        Airline.LogoUrl = string.IsNullOrWhiteSpace(Airline.LogoUrl) ? null : Airline.LogoUrl.Trim();
        Airline.Description = string.IsNullOrWhiteSpace(Airline.Description) ? null : Airline.Description.Trim();

        // Бизнес-правила
        if (!string.IsNullOrEmpty(Airline.IataCode) && FlightStore.IataCodeExists(Airline.IataCode))
            ModelState.AddModelError("Airline.IataCode", "Авиакомпания с таким кодом IATA уже существует.");

        if (Airline.FoundedYear > DateTime.Now.Year)
            ModelState.AddModelError("Airline.FoundedYear", "Год основания не может быть в будущем.");

        if (!ModelState.IsValid)
            return Page();

        Airline.Id = FlightStore.NextAirlineId();
        Airline.IsActive = true;
        FlightStore.Airlines.Add(Airline);
        return RedirectToPage("./Index");
    }
}