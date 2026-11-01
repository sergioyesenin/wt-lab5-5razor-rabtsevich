using System.ComponentModel.DataAnnotations;

namespace wt_lab5_5razor_rabtsevich.Models;

public class Airline
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Укажите название авиакомпании")]
    [StringLength(80, MinimumLength = 2, ErrorMessage = "Название: 2–80 символов")]
    [Display(Name = "Название")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Укажите код IATA")]
    [StringLength(3, MinimumLength = 2, ErrorMessage = "Код IATA: 2–3 символа")]
    [RegularExpression("^[A-Z0-9]{2,3}$", ErrorMessage = "Код IATA: только заглавные латинские буквы и цифры")]
    [Display(Name = "Код IATA")]
    public string IataCode { get; set; } = string.Empty;

    [Required(ErrorMessage = "Укажите страну")]
    [StringLength(80, MinimumLength = 2, ErrorMessage = "Страна: 2–80 символов")]
    [Display(Name = "Страна")]
    public string Country { get; set; } = string.Empty;

    [Range(1950, 2100, ErrorMessage = "Год основания: 1950–2100")]
    [Display(Name = "Год основания")]
    public int FoundedYear { get; set; } = 2000;

    [Display(Name = "Логотип (URL)")]
    [Url(ErrorMessage = "Некорректный URL")]
    public string? LogoUrl { get; set; }

    [Display(Name = "Описание")]
    [StringLength(500, ErrorMessage = "Не более 500 символов")]
    public string? Description { get; set; }

    [Display(Name = "Активна")]
    public bool IsActive { get; set; } = true;

    [Range(0.0, 5.0, ErrorMessage = "Рейтинг: от 0 до 5")]
    [Display(Name = "Рейтинг")]
    public decimal Rating { get; set; } = 4.0m;
}