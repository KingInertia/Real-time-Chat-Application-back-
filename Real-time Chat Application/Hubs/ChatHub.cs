using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Identity.Client;
using Real_time_Chat_Application.Models;
using System.Text.Json;
using System.Text.Json.Serialization;
using Real_time_Chat_Application.Data;
using Microsoft.EntityFrameworkCore;
namespace Real_time_Chat_Application.Hubs

{
    public class ChatHub : Hub
    {
        private readonly IDistributedCache _cache;
        private readonly ApplicationDbContext _context;

        public ChatHub(IDistributedCache cache, ApplicationDbContext context)
        {
            _cache = cache;
            _context = context;
        }
        /* public async Task JoinChat(UserConnection connection) { 

             await Groups.AddToGroupAsync(Context.ConnectionId, connection.ChatRoom);

             var stringConnection = JsonSerializer.Serialize(connection);

             await _cache.SetStringAsync(Context.ConnectionId, stringConnection);

             await Clients.Group(connection.ChatRoom).SendAsync("ReceiveMessage", "Admin", $"{connection.UserName} has joined!");
         }*/

        public async Task JoinChat(UserConnection connection)
        {
            // Добавление пользователя в группу
            await Groups.AddToGroupAsync(Context.ConnectionId, connection.ChatRoom);

            // Сохранение информации о подключении в кэш
            var stringConnection = JsonSerializer.Serialize(connection);
            await _cache.SetStringAsync(Context.ConnectionId, stringConnection);

            // Отправка уведомления всем пользователям в комнате о присоединении нового пользователя
            await Clients.Group(connection.ChatRoom).SendAsync("ReceiveMessage", "Admin", $"{connection.UserName} has joined!");

            // Получение всех сообщений для текущей комнаты
            var messages = await _context.Messages
                .Where(m => m.ChatRoom == connection.ChatRoom)
                .OrderBy(m => m.Timestamp)
                .ToListAsync();

            // Отправка всех сообщений текущему пользователю
            foreach (var message in messages)
            {
                await Clients.Caller.SendAsync("ReceiveMessage", message.User, message.Text);
            }
        }


        public async Task SendMessage(string message)
        {

            var stringConnection = await _cache.GetAsync(Context.ConnectionId);

            var connection = JsonSerializer.Deserialize<UserConnection>(stringConnection);

            if(connection is not null)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, connection.ChatRoom);

                // Сохранение сообщения в базу данных
                var chatMessage = new Message
                {
                    User = connection.UserName,
                    Text = message,
                    Timestamp = DateTime.UtcNow,
                   SentimentAnalysisResult = "test",
                   ChatRoom = connection.ChatRoom
                };

                _context.Messages.Add(chatMessage);
                await _context.SaveChangesAsync();


                await Clients.Group(connection.ChatRoom).SendAsync("ReceiveMessage", connection.UserName, message);
            }
            
            
        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var stringConnection = await _cache.GetAsync(Context.ConnectionId);
            var connection = JsonSerializer.Deserialize<UserConnection>(stringConnection);
            if (connection is not null) { 
                await _cache.RemoveAsync(Context.ConnectionId);
                /*await Groups.RemoveFromGroupAsync(Context.ConnectionId, connection.ChatRoom);*/

                
                await Clients.Group(connection.ChatRoom).SendAsync("ReceiveMessage", "Admin", $"{connection.UserName} has left the chat!");

            }
            await base.OnDisconnectedAsync(exception); ;
        }
    }
}
