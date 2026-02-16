using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models; // Ez kell az OpenApiInfo-hoz
using SimpleTokenGenerate.Models;
using System.Text;

namespace SimpleTokenGenerate
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<SimpletokenContext>();
            builder.Services.AddScoped<GenerateToken>();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            // --- SWAGGER KONFIGURÁCIÓ MÓDOSÍTÁSA ---
            builder.Services.AddSwaggerGen(opt =>
            {
                 opt.SwaggerDoc("v1", new OpenApiInfo { Title = "SimpleToken API by FZ, modified by NB", Version = "v1.1" });

                // Meghatározzuk a Bearer sémát
                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Másold be a tokent (Bearer nélkül, csak a kódot):",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });

                // Alkalmazzuk a védelmet globálisan a Swagger felületén
                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
          {
            new OpenApiSecurityScheme
            {
              Reference = new OpenApiReference
              {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
              }
            },
            new string[]{}
          }
        });
            });

            var secret = "Ez egy 16 karakter hosszú szoveg legalább";
            var issuer = "auth-api";
            var auidience = "auth-client";
            var key = Encoding.UTF8.GetBytes(secret);

            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = auidience
                };
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // --- NAGYON FONTOS: Authentication kell az Authorization elé! ---
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }

}
