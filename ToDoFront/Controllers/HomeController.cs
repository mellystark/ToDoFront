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
                return RedirectToAction("Index", "Todo"); // Giriþ baþarýlý ise Todo sayfasýna yönlendir
            }

            ModelState.AddModelError(string.Empty, "Giriþ baþarýsýz. Kullanýcý adý veya parola yanlýþ.");
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
                return RedirectToAction("Index", "Todo"); // Kayýt baþarýlý ise login sayfasýna yönlendir
            }
            ModelState.AddModelError(string.Empty, "Kullanýcý adý veya e-posta zaten mevcut.");
        }
        return View(model); // Kayýt baþarýsýz ise kayýt sayfasýný göster
    }
}
