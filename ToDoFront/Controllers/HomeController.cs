using Microsoft.AspNetCore.Mvc;
using ToDoFront.Models;
using ToDoFront.Services;

public class HomeController : Controller
{
    private readonly TodoApiService _todoApiService;

    public HomeController(TodoApiService todoApiService)
    {
        _todoApiService = todoApiService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginModel model)
    {
        if (ModelState.IsValid)
        {
            var response = await _todoApiService.LoginAsync(model);
            if (response.IsSuccessStatusCode)
            {
                var token = await response.Content.ReadAsStringAsync();
                HttpContext.Session.SetString("JWToken", token);
                return RedirectToAction("Index", "Todo"); // Giri� ba�ar�l� ise Todo sayfas�na y�nlendir
            }

            ModelState.AddModelError(string.Empty, "Giri� ba�ar�s�z. Kullan�c� ad� veya parola yanl��.");
        }
        return View(model);
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterModel model)
    {
        if (ModelState.IsValid)
        {
            var response = await _todoApiService.RegisterAsync(model);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Todo"); // Kay�t ba�ar�l� ise login sayfas�na y�nlendir
            }
            ModelState.AddModelError(string.Empty, "Kullan�c� ad� veya e-posta zaten mevcut.");
        }
        return View(model); // Kay�t ba�ar�s�z ise kay�t sayfas�n� g�ster
    }
}
