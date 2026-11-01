using Microsoft.AspNetCore.Mvc.RazorPages;
using wt_lab5_5razor_rabtsevich.Data;
using wt_lab5_5razor_rabtsevich.Models;

namespace wt_lab5_5razor_rabtsevich.Pages;

public class IndexModel : PageModel
{
    public int AirlinesCount { get; set; }
    public int AircraftsCount { get; set; }
    public int FlightsCount { get; set; }
    public int ActiveFlightsCount { get; set; }
    public decimal TotalActiveRevenue { get; set; }
    public List<Flight> UpcomingFlights { get; set; } = new();
    public List<Airline> TopAirlines { get; set; } = new();

    public void OnGet()
    {
        AirlinesCount = FlightStore.Airlines.Count;
        AircraftsCount = FlightStore.Aircrafts.Count;
        FlightsCount = FlightStore.Flights.Count;
        ActiveFlightsCount = FlightStore.Flights.Count(f => f.IsActive);
        TotalActiveRevenue = FlightStore.Flights
            .Where(f => f.IsActive)
            .Sum(f => f.TicketPrice * f.SeatsAvailable);

        UpcomingFlights = FlightStore.Flights
            .Where(f => f.IsActive)
            .OrderBy(f => f.DepartureTime)
            .Take(4)
            .ToList();

        TopAirlines = FlightStore.Airlines
            .OrderByDescending(a => a.Rating)
            .Take(3)
            .ToList();
    }
}