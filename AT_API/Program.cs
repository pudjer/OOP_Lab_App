using AT_Domain.Models;
using AT_Infrastructure.DbContexts;
using AT_Infrastructure.Facades;
using AT_Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

/*
 * REBRANDING REQUESTED:
 * крч дела выглядят так, что отдельные микросервисы будут взаимодействовать между собой
 * через HTTP(S), поэтому в принципе это может уже и быть отдельным решением конкретно
 * для одного микросервиса (здесь — для аутентификации и т. д.)
 */

namespace AT_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                var config = builder.Configuration;
                x.TokenValidationParameters = new()
                {
                    ValidIssuer = config["Bearer:Issuer"],
                    ValidAudience = config["Bearer:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(config["Bearer:Key"])),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                };
            });

            builder.Services.AddAuthorization();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(builder.Configuration.GetConnectionString("SQLite")));

            builder.Services.AddScoped<DbContext, AppDbContext>();
            builder.Services.AddScoped<IAuthenticationFacade, AppAuthFacade>();
            builder.Services.AddScoped<IBaseModelRepository<User>, UserRepository>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}