using Microsoft.AspNetCore.Mvc;
using ToDoFront.Models;
using ToDoFront.Services;

namespace ToDoFront.Controllers
{
    public class AccountController : Controller
    {
        private readonly TodoApiService _todoApiService;

        public AccountController(TodoApiService todoApiService)
        {
            _todoApiService = todoApiService;
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

                    // Token'ı çerezlerde veya yerel depolamada saklayın
                    HttpContext.Session.SetString("JWToken", token);

                    return RedirectToAction("Index", "Todo"); // Giriş başarılı ise Todo sayfasına yönlendir
                }

                // Hatalı giriş durumunda ViewBag ile hata mesajı ekle
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    ViewBag.ErrorMessage = "Giriş başarısız. Kullanıcı adı, email ya da parola yanlış.";
                }
                else
                {
                    // Diğer hata durumları için genel bir mesaj ekleyin
                    ViewBag.ErrorMessage = "Bir hata oluştu. Lütfen tekrar deneyin.";
                }
            }
            return View(model); // Giriş başarısız ise tekrar giriş sayfasını göster
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
                    return RedirectToAction("Login"); // Kayıt başarılı ise login sayfasına yönlendir
                }

                // Yanıtın içeriğine göre hata mesajı ekleyin
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    ViewBag.ErrorMessage = errorMessage;
                }
                else
                {
                    ViewBag.ErrorMessage = "Bir hata oluştu. Lütfen tekrar deneyin.";
                }
            }
            return View(model); // Kayıt başarısız ise kayıt sayfasını göster
        }
    }
}
