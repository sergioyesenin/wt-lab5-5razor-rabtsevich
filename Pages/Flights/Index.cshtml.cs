using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using wt_lab5_5razor_rabtsevich.Data;
using wt_lab5_5razor_rabtsevich.Models;

namespace wt_lab5_5razor_rabtsevich.Pages.Flights;

public class IndexModel : PageModel
{
    public List<Flight> Flights { get; set; } = new();
    public string? Search { get; set; }
    public int? AirlineId { get; set; }
    public string? FlightType { get; set; }
    public bool OnlyActive { get; set; }

    public List<SelectListItem> AirlineOptions { get; set; } = new();
    public List<string> FlightTypes { get; set; } = new();

    public void OnGet(string? search, int? airlineId, string? flightType, bool onlyActive = false)
    {
        Search = search;
        AirlineId = airlineId;
        FlightType = flightType;
        OnlyActive = onlyActive;

        var q = FlightStore.Flights.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(search))
            q = q.Where(f =>
                f.Destination.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                f.Departure.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                f.FlightNumber.Contains(search, StringComparison.OrdinalIgnoreCase));

        if (airlineId.HasValue)
            q = q.Where(f => f.AirlineId == airlineId.Value);

        if (!string.IsNullOrWhiteSpace(flightType))
            q = q.Where(f => f.FlightType == flightType);

        if (onlyActive)
            q = q.Where(f => f.IsActive);

        Flights = q.OrderBy(f => f.DepartureTime).ToList();

        AirlineOptions = FlightStore.Airlines
            .Select(a => new SelectListItem(a.Name, a.Id.ToString()))
            .ToList();

        FlightTypes = FlightStore.Flights
            .Select(f => f.FlightType).Distinct().OrderBy(x => x).ToList();
    }

    public IActionResult OnPostDelete(int id)
    {
        var f = FlightStore.GetFlight(id);
        if (f is not null) FlightStore.Flights.Remove(f);
        return RedirectToPage("./Index");
    }
}