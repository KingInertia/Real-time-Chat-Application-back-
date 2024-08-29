using Microsoft.Extensions.DependencyInjection;
using Real_time_Chat_Application.Hubs;
using Microsoft.EntityFrameworkCore;
using Real_time_Chat_Application.Data;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddStackExchangeRedisCache(options => {
    var connection = builder.Configuration.GetConnectionString("Redis");
    options.Configuration = connection;
});

// ��������� Entity Framework Core
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// ��������� CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000") // ����������� ���������
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials(); // ��������� ������� ������, ���� ����������
        });
});

builder.Services.AddSignalR().AddAzureSignalR(builder.Configuration.GetConnectionString("SignalR"));
var app = builder.Build();

// ���������� �������� CORS
app.UseCors("AllowSpecificOrigins");

// ��������� ���������
app.UseRouting();
app.UseAuthorization(); // ���� ������������ �����������

app.MapHub<ChatHub>("/chat");

// ������ ����������
app.Run();
