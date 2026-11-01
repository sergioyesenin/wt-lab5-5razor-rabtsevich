using System.ComponentModel.DataAnnotations;

namespace wt_lab5_5razor_rabtsevich.Models;

public class Flight
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Укажите номер рейса")]
    [StringLength(20, MinimumLength = 3, ErrorMessage = "Номер рейса: 3–20 символов")]
    [RegularExpression(@"^[A-Z0-9\-]{3,20}$",
        ErrorMessage = "Номер рейса: заглавные латинские буквы, цифры и дефис")]
    [Display(Name = "Номер рейса")]
    public string FlightNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Укажите пункт вылета")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Название: 2–100 символов")]
    [Display(Name = "Откуда")]
    public string Departure { get; set; } = string.Empty;

    [Required(ErrorMessage = "Укажите пункт назначения")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Название: 2–100 символов")]
    [Display(Name = "Куда")]
    public string Destination { get; set; } = string.Empty;

    [Required(ErrorMessage = "Укажите время вылета")]
    [Display(Name = "Время вылета")]
    [DataType(DataType.DateTime)]
    public DateTime DepartureTime { get; set; } = DateTime.Now.AddDays(1);

    [Range(0.01, 1_000_000, ErrorMessage = "Цена от 0.01 до 1 000 000")]
    [Display(Name = "Цена билета")]
    public decimal TicketPrice { get; set; }

    [Range(0, 1000, ErrorMessage = "Мест: 0–1000")]
    [Display(Name = "Свободных мест")]
    public int SeatsAvailable { get; set; }

    [Display(Name = "Рейс активен")]
    public bool IsActive { get; set; } = true;

    [Required(ErrorMessage = "Выберите авиакомпанию")]
    [Range(1, int.MaxValue, ErrorMessage = "Выберите авиакомпанию")]
    [Display(Name = "Авиакомпания")]
    public int AirlineId { get; set; }

    [Required(ErrorMessage = "Выберите самолёт")]
    [Range(1, int.MaxValue, ErrorMessage = "Выберите самолёт")]
    [Display(Name = "Самолёт")]
    public int AircraftId { get; set; }

    [Display(Name = "Тип рейса")]
    public string FlightType { get; set; } = "Пассажирский";
}