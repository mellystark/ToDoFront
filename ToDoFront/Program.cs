using ToDoFront.Controllers;
using ToDoFront.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// HttpClient ve TodoApiService'i ekleyin
builder.Services.AddHttpClient<TodoApiService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7191/"); // API'nizin temel adresi
});

// AccountController için HttpClient eklemek yerine TodoApiService'i kullanýyoruz
builder.Services.AddScoped<AccountController>();

builder.Services.AddSession(); // Session hizmetlerini ekleyin
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); // IHttpContextAccessor ekleyin

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession(); // Session middleware'i ekleyin

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
