using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using wt_lab5_5razor_rabtsevich.Data;
using wt_lab5_5razor_rabtsevich.Models;

namespace wt_lab5_5razor_rabtsevich.Pages.Flights;

public class EditModel : PageModel
{
    [BindProperty]
    public Flight Flight { get; set; } = new();

    public List<SelectListItem> Airlines { get; set; } = new();
    public List<SelectListItem> Aircrafts { get; set; } = new();
    public List<string> FlightTypes { get; set; } = new() { "Пассажирский", "Грузовой", "Чартерный" };

    public IActionResult OnGet(int id)
    {
        var existing = FlightStore.GetFlight(id);
        if (existing is null) return NotFound();

        Flight = new Flight
        {
            Id = existing.Id,
            FlightNumber = existing.FlightNumber,
            Departure = existing.Departure,
            Destination = existing.Destination,
            DepartureTime = existing.DepartureTime,
            TicketPrice = existing.TicketPrice,
            SeatsAvailable = existing.SeatsAvailable,
            IsActive = existing.IsActive,
            AirlineId = existing.AirlineId,
            AircraftId = existing.AircraftId,
            FlightType = existing.FlightType,
        };
        LoadLists();
        return Page();
    }

    public IActionResult OnPost(int id)
    {
        if (id != Flight.Id) return BadRequest();
        if (!ModelState.IsValid)
        {
            LoadLists();
            return Page();
        }

        var existing = FlightStore.GetFlight(id);
        if (existing is null) return NotFound();

        existing.FlightNumber = Flight.FlightNumber;
        existing.Departure = Flight.Departure;
        existing.Destination = Flight.Destination;
        existing.DepartureTime = Flight.DepartureTime;
        existing.TicketPrice = Flight.TicketPrice;
        existing.SeatsAvailable = Flight.SeatsAvailable;
        existing.IsActive = Flight.IsActive;
        existing.AirlineId = Flight.AirlineId;
        existing.AircraftId = Flight.AircraftId;
        existing.FlightType = Flight.FlightType;

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