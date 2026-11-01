using wt_lab5_5razor_rabtsevich.Models;

namespace wt_lab5_5razor_rabtsevich.Data;

public static class FlightStore
{
    public static List<Airline> Airlines { get; } = new()
    {
        new Airline { Id = 1, Name = "Аэрофлот", IataCode = "SU", Country = "Россия",
            FoundedYear = 1923, IsActive = true, Rating = 4.7m,
            Description = "Крупнейшая авиакомпания России, участник SkyTeam." },
        new Airline { Id = 2, Name = "S7 Airlines", IataCode = "S7", Country = "Россия",
            FoundedYear = 1957, IsActive = true, Rating = 4.5m,
            Description = "Одна из ведущих частных авиакомпаний страны." },
        new Airline { Id = 3, Name = "Emirates", IataCode = "EK", Country = "ОАЭ",
            FoundedYear = 1985, IsActive = true, Rating = 4.9m,
            Description = "Флагманская авиакомпания Дубая, крупнейший перевозчик Азии." },
        new Airline { Id = 4, Name = "Lufthansa", IataCode = "LH", Country = "Германия",
            FoundedYear = 1953, IsActive = false, Rating = 4.4m,
            Description = "Немецкий национальный перевозчик, основатель Star Alliance." },
    };

    public static List<Aircraft> Aircrafts { get; } = new()
    {
        new Aircraft { Id = 1, Model = "Airbus A320", Manufacturer = "Airbus",
            SeatCapacity = 180, ManufactureYear = 2018, AirlineId = 1,
            TailNumber = "RA-89001", IsOperational = true },
        new Aircraft { Id = 2, Model = "Boeing 737-800", Manufacturer = "Boeing",
            SeatCapacity = 189, ManufactureYear = 2016, AirlineId = 2,
            TailNumber = "VQ-BRP", IsOperational = true },
        new Aircraft { Id = 3, Model = "Airbus A380", Manufacturer = "Airbus",
            SeatCapacity = 517, ManufactureYear = 2014, AirlineId = 3,
            TailNumber = "A6-EVK", IsOperational = true },
        new Aircraft { Id = 4, Model = "Boeing 747-8", Manufacturer = "Boeing",
            SeatCapacity = 364, ManufactureYear = 2012, AirlineId = 4,
            TailNumber = "D-ABYA", IsOperational = false },
    };

    public static List<Flight> Flights { get; } = new()
    {
        new Flight { Id = 1, FlightNumber = "SU-100", Departure = "Москва",
            Destination = "Санкт-Петербург", DepartureTime = new DateTime(2025, 5, 10, 8, 30, 0),
            TicketPrice = 4500, SeatsAvailable = 120, IsActive = true,
            AirlineId = 1, AircraftId = 1, FlightType = "Пассажирский" },
        new Flight { Id = 2, FlightNumber = "SU-205", Departure = "Москва",
            Destination = "Сочи", DepartureTime = new DateTime(2025, 5, 11, 14, 0, 0),
            TicketPrice = 7800, SeatsAvailable = 45, IsActive = true,
            AirlineId = 1, AircraftId = 1, FlightType = "Пассажирский" },
        new Flight { Id = 3, FlightNumber = "S7-310", Departure = "Казань",
            Destination = "Новосибирск", DepartureTime = new DateTime(2025, 5, 12, 6, 15, 0),
            TicketPrice = 6200, SeatsAvailable = 0, IsActive = false,
            AirlineId = 2, AircraftId = 2, FlightType = "Пассажирский" },
        new Flight { Id = 4, FlightNumber = "EK-412", Departure = "Дубай",
            Destination = "Москва", DepartureTime = new DateTime(2025, 5, 13, 18, 45, 0),
            TicketPrice = 53000, SeatsAvailable = 78, IsActive = true,
            AirlineId = 3, AircraftId = 3, FlightType = "Пассажирский" },
        new Flight { Id = 5, FlightNumber = "LH-518", Departure = "Франкфурт",
            Destination = "Москва", DepartureTime = new DateTime(2025, 5, 14, 21, 10, 0),
            TicketPrice = 41000, SeatsAvailable = 12, IsActive = true,
            AirlineId = 4, AircraftId = 4, FlightType = "Пассажирский" },
        new Flight { Id = 6, FlightNumber = "SU-777", Departure = "Москва",
            Destination = "Владивосток", DepartureTime = new DateTime(2025, 5, 15, 11, 0, 0),
            TicketPrice = 18500, SeatsAvailable = 95, IsActive = true,
            AirlineId = 1, AircraftId = 1, FlightType = "Пассажирский" },
    };

    // ── helpers ──
    public static Airline? GetAirline(int id) => Airlines.FirstOrDefault(a => a.Id == id);
    public static Aircraft? GetAircraft(int id) => Aircrafts.FirstOrDefault(a => a.Id == id);
    public static Flight? GetFlight(int id) => Flights.FirstOrDefault(f => f.Id == id);

    public static IEnumerable<Flight> FlightsOfAirline(int airlineId)
        => Flights.Where(f => f.AirlineId == airlineId);

    public static IEnumerable<Aircraft> AircraftsOfAirline(int airlineId)
        => Aircrafts.Where(a => a.AirlineId == airlineId);

    public static int NextAirlineId() => Airlines.Any() ? Airlines.Max(a => a.Id) + 1 : 1;
    public static int NextAircraftId() => Aircrafts.Any() ? Aircrafts.Max(a => a.Id) + 1 : 1;
    public static int NextFlightId() => Flights.Any() ? Flights.Max(f => f.Id) + 1 : 1;

    // ── uniqueness ──
    public static bool IataCodeExists(string code, int? excludeId = null)
    {
        if (string.IsNullOrWhiteSpace(code)) return false;
        var normalized = code.Trim().ToUpperInvariant();
        return Airlines.Any(a => a.IataCode == normalized && a.Id != excludeId);
    }

    public static bool FlightNumberExists(string number, int? excludeId = null)
    {
        if (string.IsNullOrWhiteSpace(number)) return false;
        var normalized = number.Trim().ToUpperInvariant();
        return Flights.Any(f => f.FlightNumber.ToUpperInvariant() == normalized && f.Id != excludeId);
    }

    public static bool TailNumberExists(string tail, int? excludeId = null)
    {
        if (string.IsNullOrWhiteSpace(tail)) return false;
        var normalized = tail.Trim().ToUpperInvariant();
        return Aircrafts.Any(a => a.TailNumber.ToUpperInvariant() == normalized && a.Id != excludeId);
    }
}