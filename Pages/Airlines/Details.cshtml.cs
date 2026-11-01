using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using wt_lab5_5razor_rabtsevich.Data;
using wt_lab5_5razor_rabtsevich.Models;

namespace wt_lab5_5razor_rabtsevich.Pages.Airlines;

public class DetailsModel : PageModel
{
    public Airline? Airline { get; set; }
    public List<Aircraft> Aircrafts { get; set; } = new();
    public List<Flight> Flights { get; set; } = new();

    public IActionResult OnGet(int? id)
    {
        if (id is null || id <= 0)
            return BadRequest();

        Airline = FlightStore.GetAirline(id.Value);
        if (Airline is null)
            return NotFound();

        Aircrafts = FlightStore.AircraftsOfAirline(id.Value).ToList();
        Flights = FlightStore.FlightsOfAirline(id.Value).OrderBy(f => f.DepartureTime).ToList();
        return Page();
    }
}