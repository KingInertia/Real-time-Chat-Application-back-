
using Microsoft.EntityFrameworkCore;
using Real_time_Chat_Application.Models;

namespace Real_time_Chat_Application.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Определите DbSet свойства для ваших сущностей
        public DbSet<Message> Messages { get; set; }

        // Добавьте другие DbSet свойства по мере необходимости
    }
}

