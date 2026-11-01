using Microsoft.AspNetCore.Mvc.RazorPages;

namespace wt_lab5_5razor_rabtsevich.Pages;

public class ErrorModel : PageModel
{
    public int StatusCode { get; set; }
    public string? Message { get; set; }

    public void OnGet(int? code)
    {
        StatusCode = code ?? 500;

        Message = StatusCode switch
        {
            400 => "Некорректный запрос. Проверьте параметры.",
            403 => "Доступ запрещён.",
            404 => "Страница или объект не найдены.",
            405 => "Метод не поддерживается.",
            _   => "Произошла непредвиденная ошибка."
        };
    }
}