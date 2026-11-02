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

    public void OnGet() => LoadLists();

    public IActionResult OnPost()
    {
        // ── Нормализация ──
        Flight.FlightNumber = (Flight.FlightNumber ?? "").Trim().ToUpperInvariant();
        Flight.Departure = Flight.Departure?.Trim() ?? string.Empty;
        Flight.Destination = Flight.Destination?.Trim() ?? string.Empty;
        Flight.FlightType = string.IsNullOrWhiteSpace(Flight.FlightType)
            ? "Пассажирский"
            : Flight.FlightType.Trim();

        // ── FK: авиакомпания ──
        var airline = FlightStore.GetAirline(Flight.AirlineId);
        if (airline is null)
            ModelState.AddModelError("Flight.AirlineId", "Выбранная авиакомпания не найдена.");

        // ── FK: самолёт ──
        var aircraft = FlightStore.GetAircraft(Flight.AircraftId);
        if (aircraft is null)
            ModelState.AddModelError("Flight.AircraftId", "Выбранное воздушное судно не найдено.");

        // ── Согласованность aircraft ↔ airline ──
        if (airline is not null && aircraft is not null && aircraft.AirlineId != airline.Id)
            ModelState.AddModelError("Flight.AircraftId",
                "Выбранный самолёт не принадлежит выбранной авиакомпании.");

        // ── Уникальность номера рейса ──
        if (!string.IsNullOrEmpty(Flight.FlightNumber) &&
            FlightStore.FlightNumberExists(Flight.FlightNumber))
            ModelState.AddModelError("Flight.FlightNumber", "Рейс с таким номером уже существует.");

        // ── Departure != Destination ──
        if (!string.IsNullOrEmpty(Flight.Departure) &&
            string.Equals(Flight.Departure, Flight.Destination, StringComparison.OrdinalIgnoreCase))
            ModelState.AddModelError("Flight.Destination", "Пункт назначения не должен совпадать с пунктом вылета.");

        // ── Время вылета не в прошлом ──
        if (Flight.DepartureTime < DateTime.Now)
            ModelState.AddModelError("Flight.DepartureTime", "Время вылета не может быть в прошлом.");

        // ── Мест не больше вместимости самолёта ──
        if (aircraft is not null && Flight.SeatsAvailable > aircraft.SeatCapacity)
            ModelState.AddModelError("Flight.SeatsAvailable",
                $"Свободных мест не может быть больше вместимости самолёта ({aircraft.SeatCapacity}).");

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