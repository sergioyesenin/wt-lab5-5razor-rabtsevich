using Microsoft.AspNetCore.Mvc.RazorPages;
using wt_lab5_5razor_rabtsevich.Data;
using wt_lab5_5razor_rabtsevich.Models;

namespace wt_lab5_5razor_rabtsevich.Pages.Aircrafts;

public class IndexModel : PageModel
{
    public List<Aircraft> Aircrafts { get; set; } = new();
    public string? Manufacturer { get; set; }
    public List<string> Manufacturers { get; set; } = new();

    public void OnGet(string? manufacturer)
    {
        // Нормализация входного параметра
        if (!string.IsNullOrWhiteSpace(manufacturer))
        {
            manufacturer = manufacturer.Trim();
            if (manufacturer.Length > 100)
                manufacturer = manufacturer[..100];
        }
        else
        {
            manufacturer = null;
        }

        Manufacturer = manufacturer;

        var q = FlightStore.Aircrafts.AsEnumerable();
        if (!string.IsNullOrWhiteSpace(Manufacturer))
            q = q.Where(a => a.Manufacturer == Manufacturer);

        Aircrafts = q.OrderBy(a => a.Model).ToList();
        Manufacturers = FlightStore.Aircrafts
            .Select(a => a.Manufacturer)
            .Distinct()
            .OrderBy(m => m)
            .ToList();
    }
}