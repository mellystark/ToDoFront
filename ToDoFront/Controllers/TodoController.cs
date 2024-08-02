using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using ToDoFront.Models;
using ToDoFront.Services;

namespace ToDoFront.Controllers
{
    public class TodoController : Controller
    {
        private readonly TodoApiService _todoApiService;

        public TodoController(TodoApiService todoApiService)
        {
            _todoApiService = todoApiService;
        }

        public async Task<IActionResult> Index()
        {
            var todos = await _todoApiService.GetTodosAsync();
            return View(todos);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TodoItemModel model)
        {
            if (ModelState.IsValid)
            {
                // API üzerinden yeni todo ekleme işlemini gerçekleştirin
                await _todoApiService.CreateTodoAsync(model);
                return RedirectToAction("Index"); // Başarılı bir şekilde ekleme yapıldıysa ana sayfaya dön
            }
            return View(model); // Ekleme işlemi başarısızsa formu tekrar göster
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var todo = await _todoApiService.GetTodoByIdAsync(id);
            if (todo == null)
            {
                return NotFound();
            }
            return View(todo);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, bool isComplete)
        {
            if (ModelState.IsValid)
            {
                var todo = await _todoApiService.GetTodoByIdAsync(id);
                if (todo != null)
                {
                    todo.IsComplete = isComplete;
                    var response = await _todoApiService.UpdateTodoAsync(id, todo);
                    if (response.IsSuccessStatusCode)
                    {
                        return Json(new { success = true });
                    }
                }
                return Json(new { success = false, message = "Todo güncellenemedi." });
            }
            return Json(new { success = false, message = "Geçersiz model." });
        }



        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _todoApiService.DeleteTodoAsync(id);
            if (success)
            {
                TempData["SuccessMessage"] = "Todo başarıyla silindi.";
            }
            else
            {
                TempData["ErrorMessage"] = "Todo bulunamadı veya silme işlemi başarısız.";
            }
            return RedirectToAction("Index"); // Silme işleminden sonra listeye dön
        }



        // Diğer CRUD aksiyonlarını burada ekleyin
    }
}