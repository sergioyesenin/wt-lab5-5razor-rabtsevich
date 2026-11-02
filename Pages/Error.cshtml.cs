using Microsoft.AspNetCore.Mvc.RazorPages;

namespace wt_lab5_5razor_rabtsevich.Pages;

public class ErrorModel : PageModel
{
    public int Code { get; set; }
    public string? Message { get; set; }

    public void OnGet(int? code)
    {
        // Допускаем только стандартные HTTP-коды ошибок
        Code = code is >= 400 and <= 599 ? code.Value : 500;

        Message = Code switch
        {
            400 => "Некорректный запрос. Проверьте параметры.",
            401 => "Требуется авторизация.",
            403 => "Доступ запрещён.",
            404 => "Страница или объект не найдены.",
            405 => "Метод не поддерживается.",
            409 => "Конфликт данных.",
            500 => "Внутренняя ошибка сервера.",
            _   => "Произошла непредвиденная ошибка."
        };
    }
}