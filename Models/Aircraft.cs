using System.ComponentModel.DataAnnotations;

namespace wt_lab5_5razor_rabtsevich.Models;

public class Aircraft
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Укажите модель")]
    [StringLength(80, MinimumLength = 2, ErrorMessage = "Модель: 2–80 символов")]
    [Display(Name = "Модель")]
    public string Model { get; set; } = string.Empty;

    [Required(ErrorMessage = "Укажите производителя")]
    [StringLength(80, MinimumLength = 2, ErrorMessage = "Производитель: 2–80 символов")]
    [Display(Name = "Производитель")]
    public string Manufacturer { get; set; } = string.Empty;

    [Range(1, 1000, ErrorMessage = "Мест: 1–1000")]
    [Display(Name = "Пассажирских мест")]
    public int SeatCapacity { get; set; }

    [Range(1950, 2100, ErrorMessage = "Год выпуска: 1950–2100")]
    [Display(Name = "Год выпуска")]
    public int ManufactureYear { get; set; } = 2000;

    [Required(ErrorMessage = "Выберите авиакомпанию")]
    [Range(1, int.MaxValue, ErrorMessage = "Выберите авиакомпанию")]
    [Display(Name = "Авиакомпания")]
    public int AirlineId { get; set; }

    [Required(ErrorMessage = "Укажите бортовой номер")]
    [StringLength(10, MinimumLength = 3, ErrorMessage = "Бортовой номер: 3–10 символов")]
    [RegularExpression(@"^[A-Z0-9\-]{3,10}$",
        ErrorMessage = "Бортовой номер: заглавные латинские буквы, цифры и дефис")]
    [Display(Name = "Бортовой номер")]
    public string TailNumber { get; set; } = string.Empty;

    [Display(Name = "В эксплуатации")]
    public bool IsOperational { get; set; } = true;
}