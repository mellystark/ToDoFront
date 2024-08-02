using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using ToDoFront.Models;
using System.Text.Json;

namespace ToDoFront.Services
{
    public class TodoApiService
    {
        private readonly HttpClient _httpClient;

        public TodoApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Todo öğelerini almak için API'ye istek gönderin
        public async Task<IEnumerable<TodoItemModel>> GetTodosAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<TodoItemModel>>("https://localhost:7191/api/todo");
        }

        // Kullanıcı kaydı için API'ye istek gönderin
        public async Task<HttpResponseMessage> RegisterAsync(RegisterModel model)
        {
            return await _httpClient.PostAsJsonAsync("api/account/register", model);
        }

        // Kullanıcı girişi için API'ye istek gönderin
        public async Task<HttpResponseMessage> LoginAsync(LoginModel model)
        {
            return await _httpClient.PostAsJsonAsync("api/account/login", model);
        }

        public async Task<HttpResponseMessage> CreateTodoAsync(TodoItemModel model)
        {
            return await _httpClient.PostAsJsonAsync("api/todo", model);
        }


         public async Task<TodoItemModel> GetTodoByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<TodoItemModel>($"https://localhost:7191/api/todo/{id}");
        }

        public async Task<HttpResponseMessage> UpdateTodoAsync(int id, TodoItemModel model)
        {
            return await _httpClient.PutAsJsonAsync($"api/todo/{id}", model);
        }

        public async Task<bool> DeleteTodoAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/todo/{id}");
            return response.IsSuccessStatusCode;
        }


        // Diğer CRUD metotlarını burada ekleyin
    }
}
