using Microsoft.Extensions.DependencyInjection;
using Real_time_Chat_Application.Hubs;
using Microsoft.EntityFrameworkCore;
using Real_time_Chat_Application.Data;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddStackExchangeRedisCache(options => {
    var connection = builder.Configuration.GetConnectionString("Redis");
    options.Configuration = connection;
});

// Настройка Entity Framework Core
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// Настройка CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000") // Разрешенные источники
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials(); // Разрешить учетные данные, если необходимо
        });
});

builder.Services.AddSignalR().AddAzureSignalR(builder.Configuration.GetConnectionString("SignalR"));
var app = builder.Build();

// Применение политики CORS
app.UseCors("AllowSpecificOrigins");

// Настройка маршрутов
app.UseRouting();
app.UseAuthorization(); // Если используется авторизация

app.MapHub<ChatHub>("/chat");

// Запуск приложения
app.Run();
