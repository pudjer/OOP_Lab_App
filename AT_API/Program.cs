using AT_Infrastructure.DbContexts;
using AT_Infrastructure.Facades;
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

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(builder.Configuration.GetConnectionString("SQLite")));

            builder.Services.AddScoped<DbContext, AppDbContext>();
            builder.Services.AddScoped<IAuthenticationFacade, AppAuthFacade>();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateLifetime = true,
                        ValidateIssuer = true,
                        ValidIssuer = builder.Configuration.GetSection("JWT Bearer")
                            .GetValue<string>("Issuer"),
                        ValidateAudience = true,
                        ValidAudience = builder.Configuration.GetSection("JWT Bearer")
                            .GetValue<string>("Issuer"),
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(builder.Configuration
                            .GetSection("JWT Bearer")
                            .GetValue<string>("SymmetricSecurityKey"))),
                    };
                });

            var app = builder.Build();

            app.UseAuthentication();

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