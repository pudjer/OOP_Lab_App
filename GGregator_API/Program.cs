using GGregator_Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

/*
 * REBRANDING REQUESTED:
 * крч дела выглядят так, что отдельные микросервисы будут взаимодействовать между собой
 * через HTTP(S), поэтому в принципе это может уже и быть отдельным решением конкретно
 * для одного микросервиса (здесь — для аутентификации и т. д.)
 */

namespace GGregator_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<SQLiteContext>(options =>
            options.UseSqlite(builder.Configuration.GetConnectionString("SQLite")));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}