using Microsoft.AspNetCore.Mvc.RazorPages;
using wt_lab5_5razor_rabtsevich.Data;
using wt_lab5_5razor_rabtsevich.Models;

namespace wt_lab5_5razor_rabtsevich.Pages.Airlines;

public class IndexModel : PageModel
{
    public List<Airline> Airlines { get; set; } = new();
    public string? Search { get; set; }
    public string? Country { get; set; }
    public List<string> Countries { get; set; } = new();

    public void OnGet(string? search, string? country)
    {
        Search = search;
        Country = country;

        if (!string.IsNullOrWhiteSpace(search) && search.Length > 100)
            Search = search = search[..100];

        var query = FlightStore.Airlines.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(Search))
            query = query.Where(a =>
                a.Name.Contains(Search, StringComparison.OrdinalIgnoreCase) ||
                a.IataCode.Contains(Search, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(Country))
            query = query.Where(a => a.Country == Country);

        Airlines = query.OrderByDescending(a => a.Rating).ToList();
        Countries = FlightStore.Airlines.Select(a => a.Country).Distinct().OrderBy(c => c).ToList();
    }
}