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

        // Безопасные фильтры
        if (!string.IsNullOrWhiteSpace(Search) && Search.Length > 100)
            Search = search = Search[..100];

        if (AirlineId is <= 0)
            AirlineId = null;

        var q = Flight