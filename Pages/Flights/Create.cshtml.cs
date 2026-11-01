using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using wt_lab5_5razor_rabtsevich.Data;
using wt_lab5_5razor_rabtsevich.Models;

namespace wt_lab5_5razor_rabtsevich.Pages.Flights;

public class CreateModel : PageModel
{
    [BindProperty]
    public Flight Flight { get; set; } = new();

    public List<SelectListItem> Airlines { get; set; } = new();
    public List<SelectListItem> Aircrafts { get; set; } = new();
    public List<string> FlightTypes { get; set; } = new() { "Пассажирский", "Грузовой", "Чартерный" };

    public void OnGet()
    {
        LoadLists();
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            LoadLists();
            return Page();
        }

        Flight.Id = FlightStore.NextFlightId();
        FlightStore.Flights.Add(Flight);
        return RedirectToPage("./Index");
    }

    private void LoadLists()
    {
        Airlines = FlightStore.Airlines
            .Select(a => new SelectListItem(a.Name, a.Id.ToString()))
            .ToList();
        Aircrafts = FlightStore.Aircrafts
            .Select(a => new SelectListItem($"{a.Model} ({a.TailNumber})", a.Id.ToString()))
            .ToList();
    }
}