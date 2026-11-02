using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using wt_lab5_5razor_rabtsevich.Data;
using wt_lab5_5razor_rabtsevich.Models;

namespace wt_lab5_5razor_rabtsevich.Pages.Flights;

public class DetailsModel : PageModel
{
    public Flight? Flight { get; set; }
    public Airline? Airline { get; set; }
    public Aircraft? Aircraft { get; set; }

    public IActionResult OnGet(int id)
    {
        if (id <= 0) return BadRequest();

        Flight = FlightStore.GetFlight(id);
        if (Flight is null) return NotFound();

        Airline = FlightStore.GetAirline(Flight.AirlineId);
        Aircraft = FlightStore.GetAircraft(Flight.AircraftId);
        return Page();
    }
}