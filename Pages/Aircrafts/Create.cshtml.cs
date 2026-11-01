using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using wt_lab5_5razor_rabtsevich.Data;
using wt_lab5_5razor_rabtsevich.Models;

namespace wt_lab5_5razor_rabtsevich.Pages.Aircrafts;

public class CreateModel : PageModel
{
    [BindProperty]
    public Aircraft Aircraft { get; set; } = new();

    public List<SelectListItem> Airlines { get; set; } = new();

    public void OnGet() => LoadAirlines();

    public IActionResult OnPost()
    {
        // Нормализация
        Aircraft.Model = Aircraft.Model?.Trim() ?? string.Empty;
        Aircraft.Manufacturer = Aircraft.Manufacturer?.Trim() ?? string.Empty;
        Aircraft.TailNumber = (Aircraft.TailNumber ?? "").Trim().ToUpperInvariant();

        // FK
        if (!FlightStore.Airlines.Any(a => a.Id == Aircraft.AirlineId))
            ModelState.AddModelError("Aircraft.AirlineId", "Выбранная авиакомпания не найдена.");

        // Уникальность
        if (!string.IsNullOrEmpty(Aircraft.TailNumber) &&
            FlightStore.TailNumberExists(Aircraft.TailNumber))
            ModelState.AddModelError("Aircraft.TailNumber", "Самолёт с таким бортовым номером уже есть.");

        // Год не в будущем
        if (Aircraft.ManufactureYear > DateTime.Now.Year)
            ModelState.AddModelError("Aircraft.ManufactureYear", "Год выпуска не может быть в будущем.");

        if (!ModelState.IsValid)
        {
            LoadAirlines();
            return Page();
        }

        Aircraft.Id = FlightStore.NextAircraftId();
        FlightStore.Aircrafts.Add(Aircraft);
        return RedirectToPage("./Index");
    }

    private void LoadAirlines()
    {
        Airlines = FlightStore.Airlines
            .Select(a => new SelectListItem(a.Name, a.Id.ToString()))
            .ToList();
    }
}