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
        if (id <= 0) return BadRequest();

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
        // ❗ Диагностика: если сюда не доходит id — увидим 400, а не тишину
        if (id <= 0) return BadRequest($"Некорректный id маршрута: {id}");
        if (id != Flight.Id) return BadRequest($"id маршрута ({id}) != Flight.Id ({Flight.Id})");

        // ── Нормализация ──
        Flight.FlightNumber = (Flight.FlightNumber ?? "").Trim().ToUpperInvariant();
        Flight.Departure = Flight.Departure?.Trim() ?? string.Empty;
        Flight.Destination = Flight.Destination?.Trim() ?? string.Empty;
        Flight.FlightType = string.IsNullOrWhiteSpace(Flight.FlightType)
            ? "Пассажирский"
            : Flight.FlightType.Trim();

        // ── FK ──
        var airline = FlightStore.GetAirline(Flight.AirlineId);
        if (airline is null)
            ModelState.AddModelError("Flight.AirlineId", "Выбранная авиакомпания не найдена.");

        var aircraft = FlightStore.GetAircraft(Flight.AircraftId);
        if (aircraft is null)
            ModelState.AddModelError("Flight.AircraftId", "Выбранное воздушное судно не найдено.");

        if (airline is not null && aircraft is not null && aircraft.AirlineId != airline.Id)
            ModelState.AddModelError("Flight.AircraftId",
                "Выбранный самолёт не принадлежит выбранной авиакомпании.");

        // ── Уникальность с исключением текущего рейса ──
        if (!string.IsNullOrEmpty(Flight.FlightNumber) &&
            FlightStore.FlightNumberExists(Flight.FlightNumber, excludeId: Flight.Id))
            ModelState.AddModelError("Flight.FlightNumber", "Рейс с таким номером уже существует.");

        // ── Departure != Destination ──
        if (!string.IsNullOrEmpty(Flight.Departure) &&
            string.Equals(Flight.Departure, Flight.Destination, StringComparison.OrdinalIgnoreCase))
            ModelState.AddModelError("Flight.Destination", "Пункт назначения не должен совпадать с пунктом вылета.");

        // ── Время вылета не в прошлом ──
        if (Flight.DepartureTime < DateTime.Now)
            ModelState.AddModelError("Flight.DepartureTime", "Время вылета не может быть в прошлом.");

        // ── Мест не больше вместимости ──
        if (aircraft is not null && Flight.SeatsAvailable > aircraft.SeatCapacity)
            ModelState.AddModelError("Flight.SeatsAvailable",
                $"Свободных мест не может быть больше вместимости самолёта ({aircraft.SeatCapacity}).");

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